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
        public List<ComboBoxOptionHelper.ComboBoxOption> PasswordTypeOptions { get; set; } = new();
        private ComboBoxOptionHelper.ComboBoxOption? _selectedPasswordTypeOption;
        public ComboBoxOptionHelper.ComboBoxOption? SelectedPasswordTypeOption
        {
            get => _selectedPasswordTypeOption;
            set
            {
                if (value != null && SelectedUser != null)
                {
                    SelectedUser.PasswordTypeId = int.Parse(value.Key);
                }
                SetProperty(ref _selectedPasswordTypeOption, value);
            }
        }
        public List<ComboBoxOptionHelper.ComboBoxOption> SecurityClearanceOptions { get; set; } = new();
        private ComboBoxOptionHelper.ComboBoxOption? _selectedSecurityClearanceOption;
        public ComboBoxOptionHelper.ComboBoxOption? SelectedSecurityClearanceOption
        {
            get => _selectedSecurityClearanceOption;
            set
            {
                if (value != null && SelectedUser != null)
                {
                    SelectedUser.ClearanceId = int.Parse(value.Key);
                }
                SetProperty(ref _selectedSecurityClearanceOption, value);
            }
        }
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
            set
            {
                SetProperty(ref _selectedUser, value);
                if (value != null)
                {
                    SelectedPasswordTypeOption = ComboBoxOptionHelper.GetOptionByKey(value.PasswordTypeId.ToString(), PasswordTypeOptions);
                    SelectedSecurityClearanceOption = ComboBoxOptionHelper.GetOptionByKey(value.ClearanceId.ToString(), SecurityClearanceOptions);
                }
            }
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
            foreach (var passwordType in appDataService.PasswordTypes)
            {
                PasswordTypeOptions.Add(new(passwordType.Id.ToString(), passwordType.Name));
            }
            foreach (var clearance in appDataService.SecurityClearances)
            {
                SecurityClearanceOptions.Add(new(clearance.Id.ToString(), clearance.Name));
            }
            AppDataService.Users.Where(x => !x.IsAdmin).ToList().ForEach(user => Users.Add(user));
        }

        public async void SaveUserData()
        {
            if (SelectedUser != null)
            {
                SelectedUser = await AppDataService.UpdateUserData(SelectedUser);
            }
        }
    }
}