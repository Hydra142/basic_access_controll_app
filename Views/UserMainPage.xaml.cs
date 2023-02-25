using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SafeMessenge.ViewModels;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
        if (ViewModel.SelectedFile != null)
        {
            var filePath = ViewModel.SelectedFile.FilePath;
            FileEditor.Text = await File.ReadAllTextAsync(filePath);
            var canEdit = ViewModel.SelectedFile.IsWriteAble;
            FileEditor.IsReadOnly = !canEdit;
            SaveFileBtn.Visibility = canEdit? Visibility.Visible : Visibility.Collapsed;
            FileEditorGrid.Visibility = Visibility.Visible;
            //var fileStream = new FileStream(filePath + ".rtf", FileMode.Open);
            //FileEditor.Document.LoadFromStream(TextSetOptions.FormatRtf, fileStream.AsRandomAccessStream());
            //fileStream.Close();
        }
    }

    private async void SaveFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedFile != null)
        {
            var filePath = ViewModel.SelectedFile.FilePath;
            await File.WriteAllTextAsync(filePath, FileEditor.Text);
        }
    }
}
