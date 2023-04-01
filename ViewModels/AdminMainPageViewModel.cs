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
        public ObservableCollection<UserRoleItem> CurrentUserRoles = new();
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
                        _ = LoadUserRoles();
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

        //role section
        public ObservableCollection<RoleFileItem> CurrentRoleItems = new();
        public ObservableCollection<Role> Roles = new();
        private Role? _selectedRole;
        public Role? SelectedRole
        {
            get => _selectedRole;
            set
            {
                var oldSelectionId = _selectedRole?.Id;
                SetProperty(ref _selectedRole, value);
                if (value != null)
                {
                    if (value.Id != oldSelectionId)
                    {
                        _ = LoadRoleFiles();
                    }
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
            foreach (var actionType in appDataService.ActionTypes)
            {
                ActionTypeOptions.Add(new(actionType.Id.ToString(), actionType.Name));
            }
            AppDataService.Files.ForEach(file => Files.Add(file));
            AppDataService.Roles.ForEach(role => Roles.Add(role));
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
        public async Task LoadUserRoles()
        {
            if (SelectedUser != null)
            {
                CurrentUserRoles.Clear();
                foreach (var item in await AppDataService.GetUserRolesById(SelectedUser.Id))
                {
                    CurrentUserRoles.Add(item);
                }
            }
            
        }

        public async Task LoadRoleFiles()
        {
            CurrentRoleItems.Clear();
            if (SelectedRole != null  && SelectedRole.Id.HasValue)
            {
                foreach (var item in await AppDataService.GetRoleFilesById(SelectedRole.Id.Value))
                {
                    item.ActionTypesOptions = AppDataService.ActionTypes;
                    CurrentRoleItems.Add(item);
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

        public async void SaveRole()
        {
            if (SelectedRole != null)
            {
                var role = await AppDataService.InsertOrUpdateRole(SelectedRole);
                SelectedRole.Id = role.Id;
                await LoadRoleFiles();
                await LoadUserRoles();
            }
        }

        public void LoadCreateFileTemplate()
        {
            CurrentRoleItems.Clear();
            Files.Insert(0, new File() { Name = "Новий файл", MinimumClearanceId = AppDataService.SecurityClearances.First().Id });
            SelectedFile = Files[0];
        }

        public void LoadCreateRoleTemplate()
        {
            Roles.Insert(0, new Role() { Name = $"Роль {Roles.Count + 1}"});
            SelectedRole = Roles[0];
        }

        public async Task SaveUserDiscretionaryAccessMatrix()
        {
            if (SelectedUser != null)
            {
                // дістаємо всі елемети матриці у якийх адміністатор активував чекбокс
                var matrix = CurrentUserDiscretionaryMatrix.Where(x => x.IsActive).ToList();
                //змінюємо поточні занчення або додаємо нові до БД
                await AppDataService.InsertOrUpdateDiscretionaryAccessMatrixItems(matrix);
                // дістаємо айді всіх елеметів які були декативоані адміністратором
                var deleteList = CurrentUserDiscretionaryMatrix
                    .Where(x => !x.IsActive && x.Id != null)
                    .Select(x => x.Id.Value);
                //надсилаємо запит на видалення
                await AppDataService.DeleteDiscretionaryAccessMatrixItems(deleteList);
                //завантажуємо оновлену сатрицю
                await LoadUserDiscretionaryMatrix();
            }
        }

        public async Task SaveUserRoles()
        {
            if (SelectedUser != null)
            {
                // дістаємо всі елемети матриці у якийх адміністатор активував чекбокс
                var matrix = CurrentUserRoles.Where(x => x.IsActive).ToList();
                //змінюємо поточні занчення або додаємо нові до БД
                await AppDataService.InsertOrUpdateUserRoles(matrix);
                // дістаємо айді всіх елеметів які були декативоані адміністратором
                var deleteList = CurrentUserRoles
                    .Where(x => !x.IsActive && x.Id != null)
                    .Select(x => x.Id.Value);
                //надсилаємо запит на видалення
                await AppDataService.DeleteUserRoles(deleteList);
                //завантажуємо оновлену сатрицю
                await LoadUserRoles();
            }
        }

        public async Task SaveRoleFilesItems()
        {
            if (SelectedRole != null && SelectedRole.Id.HasValue)
            {
                // дістаємо всі елемети матриці у якийх адміністатор активував чекбокс
                var matrix = CurrentRoleItems.Where(x => x.IsActive).ToList();
                //змінюємо поточні занчення або додаємо нові до БД
                await AppDataService.InsertOrUpdateRoleFile(matrix);
                // дістаємо айді всіх елеметів які були декативоані адміністратором
                var deleteList = CurrentRoleItems
                    .Where(x => !x.IsActive && x.Id != null)
                    .Select(x => x.Id.Value);
                //надсилаємо запит на видалення
                await AppDataService.DeleteRoleFiles(deleteList);
                //завантажуємо оновлену сатрицю
                await LoadRoleFiles();
            }
        }
    }
    public enum AdminSection
    {
        [Display(Name = "Користувачі")]
        User,
        [Display(Name = "Файли")]
        File,
        [Display(Name = "Ролі")]
        Role
    }
}