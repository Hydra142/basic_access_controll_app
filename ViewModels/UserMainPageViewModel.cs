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
        //отримання доступних файлів з бд
        var currentFiles = await AppDataService.GetUserFiles(CurrentUser);
        //створення массиву ідентифікаторів 
        var currentFilesIds = currentFiles.Select(file => file.Id);
        //наповнення динамічної змінної
        currentFiles.ForEach(file => UserFiles.Add(file));
        //перевірка чи у користувача обрана дискреційна модель розмежування
        if (CurrentUser != null && CurrentUser.AccessControlModelId == AccessControlModel.DiscretionaryAccessControl)
        {
            var updateFilesTimer = new DispatcherTimer();
            //встановлення періодичності вконування функції (5 сек)
            updateFilesTimer.Interval = TimeSpan.FromSeconds(5);
            //визначення фкнкції яка буде виконуватись
            updateFilesTimer.Tick += async (sender, args) =>
            {
                //отримання доступних файлів з бд
                var files = await AppDataService.GetUserFiles(CurrentUser);
                //знахоженння які з файлів не були присутніми
                var newFiles = files.Where(x => !currentFilesIds.Contains(x.Id)).ToList();
                //наповнення динамічної змінної
                newFiles.ForEach(file => UserFiles.Add(file));
                //знахходження елементів матриці які вже не пристутні в результаті
                var removedFilesIds = currentFilesIds.Where(x => !files.Select(z => z.Id).Contains(x)).ToList();
                //видалення файлів з динамічної змінної
                UserFiles.Where(x => removedFilesIds.Contains(x.Id)).ToList().ForEach(x => UserFiles.Remove(x));
                //збереження нового списку ідентифікаторів
                currentFilesIds = UserFiles.Select(x => x.Id);
            };
            //запуск 
            updateFilesTimer.Start();
        }
    }
}
