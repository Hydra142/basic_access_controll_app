using Microsoft.IdentityModel.Tokens;
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
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginPage : Page
{
    private CancellationTokenSource _cancellationToken;
    private BruteForceSettings _bruteForceSettings = new BruteForceSettings();
    public LoginPageViewModel ViewModel { get; set; }
    public bool IsShowUSerSelection = false;
    public LoginPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<LoginPageViewModel>();
    }
    private async void Login(object sender, RoutedEventArgs e)
    {
        if (ViewModel.CyrrentUser != null && !ViewModel.CyrrentUser.Password.IsNullOrEmpty())
        {
            // перевірка на правильність паролю
            var isPasswordMaches = ViewModel.CyrrentUser.Password == ViewModel.Password;
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
                ViewModel.CyrrentUser.Password = ViewModel.Password;
                await ViewModel.UpdateUserPassword(ViewModel.CyrrentUser);
                NavigateToNextPage();
            } else
            {
                PasswordInput.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBlock.Text = ViewModel.CyrrentUser.PasswordTypeDescription;
            }
        }
    }

    private async void Login(string password)
    {
        if (ViewModel.CyrrentUser != null && !ViewModel.CyrrentUser.Password.IsNullOrEmpty())
        {
            // перевірка на правильність паролю
            var isPasswordMaches = ViewModel.CyrrentUser.Password == password;
            if (isPasswordMaches)
            {
                NavigateToNextPage();
            }
            else
            {
                //PasswordInput.BorderBrush = new SolidColorBrush(Colors.Red);
                //MessageBlock.Text = "Невіриний пароль!";
            }
        }
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


        StartBruteForce(DisplayBruteForceResult);
    }

    public async Task StartBruteForce(Action<BruteForceStatus> onOperationCompleted)
    {
        _cancellationToken = new CancellationTokenSource();
        try
        {
            var cracker = new BruteForcePasswordCracker(_bruteForceSettings);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, args) =>
            {
                MessageBlock.Text = $"Максимальний час: {cracker.Status.TimeEstimated};\nШвидкість: {cracker.Status.Speed} комбінацій/сек;\nК-ть комбінацій: {cracker.Status.TotalCombinations};";
            };
            timer.Start();
            var result = await Task.Run(() => cracker.Start(_cancellationToken.Token));
            timer.Stop();
            onOperationCompleted(result);
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

        MessageBlock.Text = $"Статус: {result.Message}; Час виконання: {result.TimeSpent}; Загаьна к-ть комбінацій: {result.TotalCombinations}";

        var currBackground = PasswordInput.Background;
        var currForeground = PasswordInput.Foreground;
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
}
