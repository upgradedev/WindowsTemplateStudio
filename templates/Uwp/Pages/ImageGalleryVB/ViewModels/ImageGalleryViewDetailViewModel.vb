﻿Imports Windows.Storage
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Navigation

Imports Param_ItemNamespace.Helpers
Imports Param_ItemNamespace.Core.Services
Imports Param_ItemNamespace.Services

Namespace ViewModels
    Public Class ImageGalleryViewDetailViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        Private Shared _image As UIElement

        Private _selectedImage As Object

        Private _source As ObservableCollection(Of SampleImage)

        Public Property SelectedImage As Object
            Get
                Return _selectedImage
            End Get
            Set
                [Param_Setter](_selectedImage, value)
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey, DirectCast(SelectedImage, SampleImage).ID)
            End Set
        End Property

        Public Property Source As ObservableCollection(Of SampleImage)
            Get
                Return _source
            End Get
            Set
                [Param_Setter](_source, value)
            End Set
        End Property

        Public Sub New()
            ' TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData()
        End Sub

        Public Sub SetImage(image As UIElement)
            _image = image
        End Sub

        Public Async Function InitializeAsync(sampleImageId As String, navigationMode as NavigationMode) As Task
            If Not String.IsNullOrEmpty(sampleImageId) AndAlso navigationMode = NavigationMode.New Then
                SelectedImage = Source.FirstOrDefault(Function(i) i.ID = sampleImageId)
            Else
                Dim selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync(Of String)(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey)
                If Not String.IsNullOrEmpty(selectedImageId) Then
                    SelectedImage = Source.FirstOrDefault(Function(i) i.ID = selectedImageId)
                End If
            End If
            Dim animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewViewModel.ImageGalleryViewAnimationOpen)
            animation?.TryStart(_image)
        End Function

        Public Sub SetAnimation()
            ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewViewModel.ImageGalleryViewAnimationClose, _image)
        End Sub
    End Class
End Namespace
