﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SafeMessenge.Helpers;
using SafeMessenge.ViewModels;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BCrypt.Net;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginPage : Page
{
    private CancellationTokenSource _cancellationToken;
    private static int _loginEntyCount = 0;
    private BruteForceSettings _bruteForceSettings = new BruteForceSettings();
    public LoginPageViewModel ViewModel { get; set; }
    public bool IsShowUSerSelection = false;
    public LoginPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<LoginPageViewModel>();
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(5000);
        timer.Tick += (s, args) =>
        {
            Interlocked.Exchange(ref _loginEntyCount, 0);
            //очищення повідомлення після якщо пройшло 5 секунд
            if (MessageBlock.Text == "Перевищено ліміт дозволених спроб!")
            {
                MessageBlock.Text = "";
            }
        };
        timer.Start();
        
    }
    private async void Login(object sender, RoutedEventArgs e)
    {
        if (!IsLoginAllowed())
        {
            MessageBlock.Text = "Перевищено ліміт дозволених спроб!";
        }
        else if (
            ViewModel.CyrrentUser.PasssworExpirationDate.HasValue
            && ViewModel.CyrrentUser.PasssworExpirationDate <= DateTime.Now)
        {
            MessageBlock.Text = "Термін дії паролю стік! Зверніться до адміністратора.";
        }
        else if (ViewModel.CyrrentUser != null && !ViewModel.CyrrentUser.Password.IsNullOrEmpty())
        {
            // перевірка на правильність паролю
            var isPasswordMaches = BCrypt.Net.BCrypt.Verify(ViewModel.Password, ViewModel.CyrrentUser.Password);
            if (isPasswordMaches)
            {
                NavigateToNextPage();
            } else
            {
                PasswordInput.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBlock.Text = "Невіриний пароль!";
            }
        } else if (ViewModel.CyrrentUser != null) // корстувач новий
        {
            //перевірка відповідності паролю умовам налаштування сладності
            var isPasswordValid = Regex.IsMatch(ViewModel.Password, ViewModel.CyrrentUser.PasswordValidationRegex);
            if (isPasswordValid)
            {
                //хешування паролю
                ViewModel.CyrrentUser.Password = BCrypt.Net.BCrypt.HashPassword(ViewModel.Password, BCrypt.Net.BCrypt.GenerateSalt());
                await ViewModel.UpdateUserPassword(ViewModel.CyrrentUser);
                NavigateToNextPage();
            } else
            {
                PasswordInput.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBlock.Text = ViewModel.CyrrentUser.PasswordTypeDescription;
            }
        }
    }

    static bool IsLoginAllowed()
    {
        //перевірка чи не було забагато спроб за останні 5 сек
        if (_loginEntyCount < 3)
        {
            //вхід дозволено - збільшуємо к-ть спроб
            Interlocked.Increment(ref _loginEntyCount);
            return true;
        }

        return false;
    }


    private void NavigateToNextPage()
    {
        if (ViewModel.CyrrentUser.IsAdmin)
        {
            ViewModel.NavigationService.NavigateToAdminMainPage();
        }
        else
        {
            ViewModel.NavigationService.NavigateToUserMainPage();
        }
    }

    private async void StartBruteForce_Click(object sender, RoutedEventArgs e)
    {
        _bruteForceSettings.Password = ViewModel.CyrrentUser.Password;
        StartBruteForceBtn.Visibility = Visibility.Collapsed;
        BruteForceSettingsBtn.Visibility = Visibility.Collapsed;
        StopBruteForceBtn.Visibility = Visibility.Visible;
        LoadingIcon.Visibility = Visibility.Visible;


        StartBruteForce();
    }

    public async Task StartBruteForce()
    {
        _cancellationToken = new CancellationTokenSource();
        try
        {
            // створення екземпляру класу для підбору передавши налаштування
            var cracker = new BruteForcePasswordCracker(_bruteForceSettings);
            DispatcherTimer timer = new DispatcherTimer();
            //запуск таймера який виведе статус підбору
            timer.Interval = TimeSpan.FromMilliseconds(1200);
            timer.Tick += (s, args) =>
            {
                if (cracker != null && cracker.Status.FoundPassword.IsNullOrEmpty())
                {
                    MessageBlock.Text =
                    $"Максимальний час: {cracker?.Status?.TimeEstimated};\n" +
                    $"Швидкість: {cracker?.Status?.Speed.ToString("N0")} комбінацій/сек;\n" +
                    $"К-ть комбінацій: {cracker?.Status?.TotalCombinations.ToString("N0")};" +
                    $"{_bruteForceSettings.ToString()}";
                }
                timer.Stop();
            };
            timer.Start();
            //запуск підбору в окремому потоці
            var result = await Task.Run(() => cracker.Start(_cancellationToken.Token));
            // відображення результату
            DisplayBruteForceResult(result);
            cracker = null;
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void DisplayBruteForceResult(BruteForceStatus result)
    {
        ViewModel.Password = result.FoundPassword;
        StartBruteForceBtn.Visibility = Visibility.Visible;
        BruteForceSettingsBtn.Visibility = Visibility.Visible;
        StopBruteForceBtn.Visibility = Visibility.Collapsed;
        LoadingIcon.Visibility = Visibility.Collapsed;

        MessageBlock.Text = 
            $"Статус: {result.Message}; " +
            $"Час виконання: {result.TimeSpent}{(result.TimeEstimated.IsNullOrEmpty()? "": $"/{result.TimeEstimated}")}; " +
            $"Загаьна к-ть комбінацій: {result.TotalCombinations.ToString("N0")}" +
            $"{_bruteForceSettings.ToString()}";

        var currBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(15, 255, 255, 255));
        var currForeground = new SolidColorBrush(Colors.White);
        PasswordInput.Background = new SolidColorBrush(result.Message == "Успіх" ? Colors.YellowGreen : Colors.Red);
        PasswordInput.Foreground = new SolidColorBrush(Colors.Red);
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(3);
        timer.Tick += (s, args) =>
        {
            PasswordInput.Background = currBackground;
            PasswordInput.Foreground = currForeground;
            timer.Stop();
        };
        timer.Start();
    }

    private void StopBruteForce_Click(object sender, RoutedEventArgs e)
    {
        _cancellationToken.Cancel();
        StartBruteForceBtn.Visibility = Visibility.Visible;
        BruteForceSettingsBtn.Visibility = Visibility.Visible;
        StopBruteForceBtn.Visibility = Visibility.Collapsed;
        LoadingIcon.Visibility = Visibility.Collapsed;
    }

    private async void BruteForceSettingsBtn_Click(object sender, RoutedEventArgs e)
    {
        _bruteForceSettings = await SettingsDialog.ShowAsync(_bruteForceSettings);
    }

    private void PasswordInput_Paste(object sender, TextControlPasteEventArgs e)
    {
        PasswordInput.BorderBrush = new SolidColorBrush(Colors.Red);
        MessageBlock.Text = "Вставлення заборонено!";
        e.Handled = true;
    }
}
