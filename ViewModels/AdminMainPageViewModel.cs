using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using SafeMessenge.Helpers;
using SafeMessenge.Models;
using SafeMessenge.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace SafeMessenge.ViewModels
{
    public class AdminMainPageViewModel : ObservableRecipient
    {
        public NavigationService NavigationService { get; set; }
        public AppDataService AppDataService { get; set; }
        private User _CurrentUser = new();
        private User? _selectedUser;
        public User CurrentUser 
        {
            get => _CurrentUser;
            set => SetProperty(ref _CurrentUser, value);
        }
        public ObservableCollection<User> Users = new();

        public User? SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }





        public AdminMainPageViewModel(NavigationService navigationService, AppDataService appDataService)
        {
            NavigationService = navigationService;
            AppDataService = appDataService;
            var currentUser = AppDataService.CurrentUser;
            if (currentUser != null)
            {
                CurrentUser = currentUser;
            } else
            {
                NavigationService.NavigateToLoginPage();
            }
            AppDataService.Users.ForEach(user => Users.Add(user));
        }
    }
}