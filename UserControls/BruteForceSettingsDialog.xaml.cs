// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SafeMessenge.Helpers;
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

public sealed partial class BruteForceSettingsDialog : UserControl
{
    private const string LowerLatinChars = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperLatinChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowerCyrillicChars = "àáâã´äåºæçè³¿éêëìíîïðñòóôõö÷øùüþÿ";
    private const string UpperCyrillicChars = "ÀÁÂÃ¥ÄÅªÆÇÈ²¯ÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÜÞß";
    private const string SpecialChars = "!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?`~";
    private const string DigitChars = "0123456789";
    public BruteForceSettings? BruteForceSettings;

    private bool _isManualTextChange = false;
    public BruteForceSettingsDialog()
    {
        this.InitializeComponent();
    }

    public async Task<BruteForceSettings> ShowAsync(BruteForceSettings bruteForceSettings)
    {
        BruteForceSettings = bruteForceSettings;
        Characters.Text = bruteForceSettings.Characters;
        LengthInput.Value = bruteForceSettings.Length;
        UpdateCheckboxStates();
        await Dialog.ShowAsync();
        bruteForceSettings.Characters = Characters.Text;
        return bruteForceSettings;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Dialog.Hide();
    }

    private void CheckBoxChanged(object sender, RoutedEventArgs e)
    {
        if (_isManualTextChange)
        {
            return;
        }

        var checkBox = sender as CheckBox;

        if (checkBox.IsChecked.HasValue)
        {
            switch (checkBox.Name)
            {
                case "LowerLatinCheckBox":
                    Characters.Text = AddOrRemoveChars(Characters.Text, LowerLatinChars, checkBox.IsChecked.Value);
                    break;
                case "UpperLatinCheckBox":
                    Characters.Text = AddOrRemoveChars(Characters.Text, UpperLatinChars, checkBox.IsChecked.Value);
                    break;
                case "LowerCyrillicCheckBox":
                    Characters.Text = AddOrRemoveChars(Characters.Text, LowerCyrillicChars, checkBox.IsChecked.Value);
                    break;
                case "UpperCyrillicCheckBox":
                    Characters.Text = AddOrRemoveChars(Characters.Text, UpperCyrillicChars, checkBox.IsChecked.Value);
                    break;
                case "SpecialCheckBox":
                    Characters.Text = AddOrRemoveChars(Characters.Text, SpecialChars, checkBox.IsChecked.Value);
                    break;
                case "DigitsCheckBox":
                    Characters.Text = AddOrRemoveChars(Characters.Text, DigitChars, checkBox.IsChecked.Value);
                    break;
            }
        }

        

    }
    private void Characters_TextChanged(object sender, TextChangedEventArgs e)
    {
        Characters.Text = new string(Characters.Text.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Distinct().ToArray());
        UpdateCheckboxStates();
    }

    private void UpdateCheckboxStates()
    {
        _isManualTextChange = true;
        Characters.Text = Characters.Text == "" ? LowerLatinChars : Characters.Text;
        LowerLatinCheckBox.IsChecked = LowerLatinChars.All(ch => Characters.Text.Contains(ch));
        UpperLatinCheckBox.IsChecked = UpperLatinChars.All(ch => Characters.Text.Contains(ch));
        LowerCyrillicCheckBox.IsChecked = LowerCyrillicChars.All(ch => Characters.Text.Contains(ch));
        UpperCyrillicCheckBox.IsChecked = UpperCyrillicChars.All(ch => Characters.Text.Contains(ch));
        SpecialCheckBox.IsChecked = SpecialChars.All(ch => Characters.Text.Contains(ch));
        DigitsCheckBox.IsChecked = DigitChars.All(ch => Characters.Text.Contains(ch));
        _isManualTextChange = false;
    }

    private string AddUniqueChars(string currentChars, string charsToAdd)
    {
        foreach (char ch in charsToAdd)
        {
            if (!currentChars.Contains(ch))
            {
                currentChars += ch;
            }
        }

        return currentChars;
    }

    private string RemoveChars(string currentChars, string charsToRemove)
    {
        return new string(currentChars.Except(charsToRemove).ToArray());
    }

    private string AddOrRemoveChars(string currentChars, string chars, bool isAdd)
    {
        if (!isAdd)
        {
            return new string(currentChars.Except(chars).ToArray());
        } else
        {
            foreach (char ch in chars)
            {
                if (!currentChars.Contains(ch))
                {
                    currentChars += ch;
                }
            }

            return currentChars;
        }
    }

    private void IsLengthKnownCheckBox_Click(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        
        var isChecked = checkBox.IsChecked.Value;
        IsLengthApproximateCheckBox.Visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;
        LengthInput.Visibility = isChecked ? Visibility.Visible : Visibility.Collapsed;
    }

    private void IsLengthApproximateCheckBox_Click(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
    }

    private void LengthInput_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {

    }
}

