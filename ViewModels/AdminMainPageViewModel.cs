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
using System.ComponentModel.DataAnnotations;
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
        public readonly AdminSection[] AdminSections =(AdminSection[]) Enum.GetValues(typeof(AdminSection));
        public readonly AccessControlModel[] AccessControlModels = 
            (AccessControlModel[]) Enum.GetValues(typeof(AccessControlModel));
        private User _currentUser = new();
        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }
        //user section
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
        public List<ComboBoxOptionHelper.ComboBoxOption> ActionTypeOptions { get; set; } = new();
        private ComboBoxOptionHelper.ComboBoxOption? _selectedActionTypeOption;
        public ComboBoxOptionHelper.ComboBoxOption? SelectedActionTypeOption
        {
            get => _selectedActionTypeOption;
            set
            {
                if (value != null && SelectedUser != null)
                {
                    SelectedUser.ActionTypeId = int.Parse(value.Key);
                }
                SetProperty(ref _selectedActionTypeOption, value);
            }
        }
        public ObservableCollection<User> Users = new();
        public ObservableCollection<DiscretionaryMatrixItem> CurrentUserDiscretionaryMatrix = new();
        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                var oldSelectionId = _selectedUser?.Id;
                SetProperty(ref _selectedUser, value);
                if (value != null)
                {

                    SelectedActionTypeOption = ComboBoxOptionHelper.GetOptionByKey(value.ActionTypeId.ToString(), ActionTypeOptions);
                    SelectedPasswordTypeOption = ComboBoxOptionHelper.GetOptionByKey(value.PasswordTypeId.ToString(), PasswordTypeOptions);
                    SelectedSecurityClearanceOption = ComboBoxOptionHelper.GetOptionByKey(value.ClearanceId.ToString(), SecurityClearanceOptions);
                    if (value.Id != oldSelectionId)
                    {
                        _ = LoadUserDiscretionaryMatrix();
                    }
                }
            }
        }

        //file section
        public ObservableCollection<File> Files = new();
        private File? _selectedFile;
        public File? SelectedFile
        { 
            get => _selectedFile;
            set
            {
                SetProperty(ref _selectedFile, value);
                if (value != null)
                {
                    SelectedFileSecurityClearanceOption = ComboBoxOptionHelper.GetOptionByKey(value.MinimumClearanceId.ToString(), SecurityClearanceOptions);
                }
            }
        }

        private ComboBoxOptionHelper.ComboBoxOption? _selectedFileSecurityClearanceOption;
        public ComboBoxOptionHelper.ComboBoxOption? SelectedFileSecurityClearanceOption
        {
            get => _selectedFileSecurityClearanceOption;
            set
            {
                if (value != null && SelectedFile != null)
                {
                    SelectedFile.MinimumClearanceId = int.Parse(value.Key);
                }
                SetProperty(ref _selectedFileSecurityClearanceOption, value);
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
            foreach (var actionType in appDataService.ActionTypes)
            {
                ActionTypeOptions.Add(new(actionType.Id.ToString(), actionType.Name));
            }
            AppDataService.Files.ForEach(file => Files.Add(file));
            AppDataService.Users.Where(x => !x.IsAdmin).ToList().ForEach(user => Users.Add(user));
        }
        public async Task LoadUserDiscretionaryMatrix()
        {
            if (SelectedUser != null)
            {
                CurrentUserDiscretionaryMatrix.Clear();
                foreach (var item in await AppDataService.GetUserDiscretionaryAccessMatrixById(SelectedUser.Id))
                {
                    item.ActionTypesOptions = AppDataService.ActionTypes;
                    CurrentUserDiscretionaryMatrix.Add(item);
                }
            }
            
        }

        public async void SaveUserData()
        {
            if (SelectedUser != null)
            {
                SelectedUser = await AppDataService.UpdateUserData(SelectedUser);
            }
        }
        public async void SaveFile()
        {
            if (SelectedFile != null)
            {
                var file = await AppDataService.InsertOrUpdateFile(SelectedFile);
                await LoadUserDiscretionaryMatrix();
            }
        }

        public void LoadCreateFileTemplate()
        {
            Files.Insert(0, new File() { Name = "Новий файл", MinimumClearanceId = AppDataService.SecurityClearances.First().Id });
            SelectedFile = Files[0];
        }

        public async Task SaveUserDiscretionaryAccessMatrix()
        {
            if (SelectedUser != null)
            {
                var matrix = CurrentUserDiscretionaryMatrix.Where(x => x.IsActive).ToList();
                await AppDataService.InsertOrUpdateDiscretionaryAccessMatrixItems(matrix);
                var deleteList = CurrentUserDiscretionaryMatrix.Where(x => !x.IsActive && x.Id != null).Select(x => x.Id.Value);
                await AppDataService.DeleteDiscretionaryAccessMatrixItems(deleteList);
                await LoadUserDiscretionaryMatrix();
            }
        }
    }
    public enum AdminSection
    {
        [Display(Name = "Користувачі")]
        User,
        [Display(Name = "Файли")]
        File
    }
}