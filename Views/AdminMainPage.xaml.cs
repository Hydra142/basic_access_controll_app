﻿using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SafeMessenge.Models;
using SafeMessenge.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminMainPage : Page
    {
        public AdminMainPageViewModel ViewModel { get; set; }
        public AdminMainPage()
        {
            ViewModel = App.GetService<AdminMainPageViewModel>();
            this.InitializeComponent();
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.NavigateToLoginPage();
        }

        private async void CreateUser(object sender, RoutedEventArgs e)
        {
            var newUserData = await CreateUserDialog2.ShowAsync(ViewModel.AppDataService.PasswordTypes);
            if (newUserData != null)
            {
                var newUser = await ViewModel.AppDataService.CreateUser(newUserData);
                if (newUser != null)
                {
                    ViewModel.Users.Insert(0, newUser);
                    var a = 5;
                }
            }
        }
    }
}