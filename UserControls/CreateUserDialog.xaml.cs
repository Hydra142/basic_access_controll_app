using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SafeMessenge.Helpers;
using SafeMessenge.Models;
using SafeMessenge.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SafeMessenge.UserControls;

public sealed partial class CreateUserDialog : UserControl
{
    public List<ComboBoxOptionHelper.ComboBoxOption> PasswordTypeOptions { get; set; } = new();
    public ComboBoxOptionHelper.ComboBoxOption? SelectedPasswordTypeOption { get; set; }
    public string? UserName { get; set; }
    public User? NewUserData { get; set; }
    public CreateUserDialog()
    {
        this.InitializeComponent();
    }

    public async Task<User?> ShowAsync(List<PasswordType> passwordTypes)
    {
        foreach (var passwordType in passwordTypes)
        {
            PasswordTypeOptions.Add(new(passwordType.Id.ToString(), passwordType.Name));
        }
        await Dialog.ShowAsync();
        await Task.CompletedTask;
        return NewUserData;
    }

    private void CreateUser(object sender, RoutedEventArgs e)
    {
        if (!UserName.IsNullOrEmpty() && SelectedPasswordTypeOption != null && !SelectedPasswordTypeOption.Key.IsNullOrEmpty())
        {
            NewUserData = new()
            {
                Name = UserName,
                PasswordTypeId = int.Parse(SelectedPasswordTypeOption.Key)
            };
        }
        Dialog.Hide();
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        Dialog.Hide();
    }
}
