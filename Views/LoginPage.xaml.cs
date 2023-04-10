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
    public LoginPageViewModel ViewModel { get; set; }
    public bool IsShowUSerSelection = false;
    public LoginPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<LoginPageViewModel>();
    }
    private async void Login(object sender, RoutedEventArgs e)
    {
        var a = new Counter();
        var timeStarted = DateTime.Now;
        //await a.StartIncreasers();
        System.Diagnostics.Debug.WriteLine($"fiinished {DateTime.Now.Subtract(timeStarted).TotalSeconds}");
        var cracker = new BruteForcePasswordCracker();
        await cracker.Start();
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
                LoginErrorMessageBlock.Text = "Невіриний пароль!";
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
                LoginErrorMessageBlock.Text = ViewModel.CyrrentUser.PasswordTypeDescription;
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
}

public class Counter
{
    public int Count { get; private set; }

    public async Task MyLongAction()
    {
        
            await Task.Delay(1000);
        
    }

    public async Task StartIncreasers()
    {
        var tasks = new Task[100];

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() => MyLongAction());
        }
        var timeStarted = DateTime.Now;
        await Task.WhenAll(tasks);
        System.Diagnostics.Debug.WriteLine($"fiinished {DateTime.Now.Subtract(timeStarted).TotalSeconds}");
    }
}
