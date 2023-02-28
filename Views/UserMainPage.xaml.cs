using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using SafeMessenge.ViewModels;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.Views;

public sealed partial class UserMainPage : Page
{
    public UserMainPageViewModel ViewModel { get; set; }
    public UserMainPage()
    {
        ViewModel = App.GetService<UserMainPageViewModel>();
        this.InitializeComponent();
    }
    private void BackToLogin(object sender, RoutedEventArgs e)
    {
        ViewModel.NavigationService.NavigateToLoginPage();
    }

    private async void OpenEditUserDialog(object sender, RoutedEventArgs e)
    {
        var user = await EditUserDialog.ShowAsync(ViewModel.CurrentUser);
        if (user != null)
        {
           ViewModel.CurrentUser = await ViewModel.AppDataService.UpdateUserData(user);
        }
    }

    private async void UserFilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TxtFileEditorGrid.Visibility = Visibility.Collapsed;
        SaveTxtFileBtn.Visibility = Visibility.Collapsed;
        ImgFileEditorGrid.Visibility = Visibility.Collapsed;
        EditImgFileBtn.Visibility = Visibility.Collapsed;
        ExecutableFileGrid.Visibility = Visibility.Collapsed;
        ExecuteFileBtn.Visibility = Visibility.Collapsed;
        if (ViewModel.SelectedFile != null)
        {
            var selectedFile = ViewModel.SelectedFile;
            var filePath = ViewModel.SelectedFile.FilePath;
            var canEdit = ViewModel.SelectedFile.IsWriteAble;
            var canExecute = ViewModel.SelectedFile.IsExecuteAble;
            if (selectedFile.FileType == Models.FileType.Txt)
            {
                FileEditor.Text = await File.ReadAllTextAsync(filePath);
                FileEditor.IsReadOnly = !canEdit;
                SaveTxtFileBtn.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
                TxtFileEditorGrid.Visibility = Visibility.Visible;
            }
            else if (selectedFile.FileType == Models.FileType.Img)
            {
                ImgFileEditorGrid.Visibility = Visibility.Visible;
                EditImgFileBtn.Visibility = canEdit ? Visibility.Visible : Visibility.Collapsed;
                var storageFile = await StorageFile.GetFileFromPathAsync(filePath);
                await LoadImageContent(storageFile);
            }
            else if (selectedFile.FileType == Models.FileType.Exe)
            {
                ExecutableFileGrid.Visibility = Visibility.Visible;
                ExecuteFileBtn.Visibility = canExecute ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }

    private async Task LoadImageContent(StorageFile file)
    {
        if (file != null)
        {
            var _bitmapImage = new BitmapImage();
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                await _bitmapImage.SetSourceAsync(stream);
            }
            ImageControl.Source = _bitmapImage;
        }
    }

    private async void SaveTxtFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedFile != null)
        {
            var filePath = ViewModel.SelectedFile.FilePath;
            await File.WriteAllTextAsync(filePath, FileEditor.Text);
        }
    }
    private async void EditImgFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedFile != null)
        {
            var currentBitmapImage = ImageControl.Source as BitmapImage;
            var filePicker = new FileOpenPicker();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");

            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                using (var stream = await file.OpenReadAsync())
                {
                    var decoder = await BitmapDecoder.CreateAsync(stream);
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(stream);
                    ImageControl.Source = bitmap;
                }

                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(ImageControl);
                var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

                try
                {
                    var tempFile = await StorageFile.GetFileFromPathAsync(ViewModel.SelectedFile.FilePath);
                    using (var stream = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, 96, 96, pixelBuffer.ToArray());
                        await encoder.FlushAsync();
                    }

                    using (var stream = await tempFile.OpenReadAsync())
                    {
                        var decoder = await BitmapDecoder.CreateAsync(stream);
                        var bitmap = new BitmapImage();
                        bitmap.SetSource(stream);
                        ImageControl.Source = bitmap;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
    private async void ExecuteFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedFile != null)
        {
            var filePath = ViewModel.SelectedFile.FilePath;
            Process.Start(filePath);
        }
    }
}
