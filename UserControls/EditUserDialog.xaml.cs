using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SafeMessenge.Models;
using SafeMessenge.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.UserControls;

public sealed partial class EditUserDialog : UserControl
{
    private AppDataService? _appDataService;
    public User? UserData { get; set; }
    public EditUserDialog()
    {
        this.InitializeComponent();
    }
    public async Task<User?> ShowAsync(User? userData, AppDataService appDataService)
    {
        _appDataService = appDataService;
        UserData = await appDataService.GetUserData(userData);
        RefreshData();
        await Dialog.ShowAsync();
        await Task.CompletedTask;
        return UserData;
    }

    private void RefreshData()
    {
        if (UserData != null)
        {
            UserName.Text = UserData._Name;
            UserPassword.Text = "";
            PasswordExpiration.Text = UserData.PasssworExpirationDate.ToString();
        }
    }

    private async void EditUser(object sender, RoutedEventArgs e)
    {
        if (UserData != null)
        {
            var isPasswordValid = Regex.IsMatch(UserData.Password, UserData.PasswordValidationRegex);
            if (!isPasswordValid)
            {
                UserPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                ErrorMessage.Text = UserData.PasswordTypeDescription;
            } else
            {
                try
                {
                    var newPassword = UserData.Password;
                    //паролі користувача з PasswordHistory 
                    var lastThreePasswordHashes = await _appDataService.GetLastUserPasswords(UserData.Id);
                    foreach (string passwordHash in lastThreePasswordHashes.FindAll(x => !x.IsNullOrEmpty()))
                    {
                        //перевіряємо чи один з старих паролів рівний новому паролю
                        if (BCrypt.Net.BCrypt.Verify(newPassword, passwordHash))
                        {
                            throw new Exception("Новий пароль не повинен збігатися з жодним із останніх 3 паролів");
                        }
                    }
                    //хешування паролю і збереження
                    UserData.Password = BCrypt.Net.BCrypt.HashPassword(UserData.Password, BCrypt.Net.BCrypt.GenerateSalt());
                    UserData = await _appDataService.UpdateUserData(UserData);
                    UserPassword.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    ErrorMessage.Text = "";
                    Dialog.Hide();
                }
                catch (Exception ex)
                {
                    UserPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                    ErrorMessage.Text = ex.Message;
                }
            }
        }
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        UserData = null;
        Dialog.Hide();
    }

    private void UserPassword_Paste(object sender, TextControlPasteEventArgs e)
    {
        UserPassword.BorderBrush = new SolidColorBrush(Colors.Red);
        ErrorMessage.Text = "Вставлення заборонено!";
        e.Handled = true;
    }
}
