using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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
    public CreateUserDialog()
    {
        this.InitializeComponent();
    }

    public async Task ShowAsync()
    {
        await Dialog.ShowAsync();
        await Task.CompletedTask;
    }

    private void CloseDialog(object sender, RoutedEventArgs e)
    {
        Dialog.Hide();
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        Dialog.Hide();
    }
}
