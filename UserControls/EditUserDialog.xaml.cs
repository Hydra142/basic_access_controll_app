using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SafeMessenge.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.UserControls;

public sealed partial class EditUserDialog : UserControl
{
    public User? UserData { get; set; }
    public EditUserDialog()
    {
        this.InitializeComponent();
    }
    public async Task<User?> ShowAsync(User? userData)
    {
        UserData = userData;
        RefreshData();
        await Dialog.ShowAsync();
        await Task.CompletedTask;
        return UserData;
    }

    private void RefreshData()
    {
        if (UserData != null)
        {
            UserName.Text = UserData.Name;
            UserPassword.Text = UserData.Password;
        }
    }

    private void EditUser(object sender, RoutedEventArgs e)
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
                UserPassword.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ErrorMessage.Text = "";
                Dialog.Hide();
            }
        }
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        UserData = null;
        Dialog.Hide();
    }
}
