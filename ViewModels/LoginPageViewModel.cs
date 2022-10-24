using CommunityToolkit.Mvvm.ComponentModel;
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
    private User? _CyrrentUser;
    private List<User> _users;

    public User? CyrrentUser
    {
        get => _CyrrentUser;
        set
        {
            AppDataService.CurrentUser = value;
            SetProperty(ref _CyrrentUser, value);
        }
    }
    public List<User> Users 
    {
        get => _users;
        set => SetProperty(ref _users, value);
    }


    public LoginPageViewModel(NavigationService navigationService, AppDataService appDataService)
    {
        NavigationService = navigationService;
        AppDataService = appDataService;
        AppDataService.SetUsersFromApi();
        _users = AppDataService.Users;
        var oldUser = AppDataService.CurrentUser;
        if (oldUser != null)
        {
            oldUser = Users.Find(user => user.Id == oldUser.Id);
            _CyrrentUser = oldUser;
        }
    }
}
