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
    public class MessengerViewModel : ObservableRecipient
    {
        public NavigationService NavigationService { get; set; }
        public AppDataService AppDataService { get; set; }
        private User _CurrentUser = new();
        private List<User> _Users = new();
        public User CurrentUser 
        {
            get => _CurrentUser;
            set => SetProperty(ref _CurrentUser, value);
        }
        public List<User> Users
        {
            get => _Users;
            set => SetProperty(ref _Users, value);
        }
        
        

        public MessengerViewModel(NavigationService navigationService, AppDataService appDataService)
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
            _Users = AppDataService.Users;
        }
    }
}