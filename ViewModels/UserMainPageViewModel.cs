using CommunityToolkit.Mvvm.ComponentModel;
using SafeMessenge.Models;
using SafeMessenge.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SafeMessenge.ViewModels;

public class UserMainPageViewModel : ObservableRecipient
{
    public NavigationService NavigationService { get; set; }
    public AppDataService AppDataService { get; set; }
    private User? _cyrrentUser;
    public User? CurrentUser
    {
        get => _cyrrentUser;
        set => SetProperty(ref _cyrrentUser, value);
    }
    public ObservableCollection<File> UserFiles = new();
    private File? _selectedFile;
    public File? SelectedFile
    {
        get => _selectedFile;
        set => SetProperty(ref _selectedFile, value);
    }
    private string _currentTime;
    public string CurrentTime
    {
        get { return _currentTime; }
        set { SetProperty(ref _currentTime, value); }
    }

    public UserMainPageViewModel(NavigationService navigationService, AppDataService appDataService)
    {
        NavigationService = navigationService;
        AppDataService = appDataService;
        CurrentUser = AppDataService.CurrentUser;
        _currentTime = DateTime.Now.ToString("HH:mm:ss");
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, args) =>
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        };
        timer.Start();
    }

    public async Task PageLoaded()
    {
        var files = await AppDataService.GetUserFiles(CurrentUser);
        files.ForEach(file => UserFiles.Add(file));
    }
}
