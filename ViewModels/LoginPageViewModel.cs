using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using SafeMessenge.Models;
using SafeMessenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.ViewModels;

public class LoginPageViewModel : ObservableRecipient
{
    public NavigationService NavigationService { get; set; }
    public AppDataService AppDataService { get; set; }
    private User? _cyrrentUser;
    private List<User> _users = new();
    private string? _password;
    private bool _IsUserSelected = false;
    public bool IsUserSelected
    {
        get => _IsUserSelected;
        set => SetProperty(ref _IsUserSelected, value);
    }

    public User? CyrrentUser
    {
        get => _cyrrentUser;
        set
        {
            IsUserSelected = value != null;
            AppDataService.CurrentUser = value;
            SetProperty(ref _cyrrentUser, value);
        }
    }
    public List<User> Users 
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }

    public string? Password 
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }


    public LoginPageViewModel(NavigationService navigationService, AppDataService appDataService)
    {
        NavigationService = navigationService;
        AppDataService = appDataService;
        Password = "q";
    }

    public async Task PageLoaded()
    {
        await AppDataService.SetUsers();
        Users = AppDataService.Users;
        var oldUser = AppDataService.CurrentUser;
        if (oldUser != null)
        {
            oldUser = Users.Find(user => user.Id == oldUser.Id);
            IsUserSelected = oldUser != null;
            CyrrentUser = oldUser;
        }
    }

    public async Task UpdateUserPassword(User user)
    {
        var result = await AppDataService.UpdateUserPassword(user);
        if (result != null)
        {
            AppDataService.Users[AppDataService.Users.IndexOf(user)] = result;
        }
    }
}
