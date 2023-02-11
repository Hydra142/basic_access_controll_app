using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SafeMessenge.Helpers;
using System;
using System.Collections.Generic;

namespace SafeMessenge.UserControls;
public sealed partial class CustomComboBox : UserControl
{
    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged;
    public event EventHandler<RoutedEventArgs>? ComboBoxLostFocus;
    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register("DisplayMemberPathProperty", typeof(string), typeof(CustomComboBox), new PropertyMetadata(""));
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(string), typeof(CustomComboBox), new PropertyMetadata(""));
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource", typeof(string), typeof(CustomComboBox), new PropertyMetadata(""));

    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }
    public ComboBoxOptionHelper.ComboBoxOption? SelectedItem
    {
        get => (ComboBoxOptionHelper.ComboBoxOption?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public List<ComboBoxOptionHelper.ComboBoxOption>? ItemsSource
    {
        get => (List<ComboBoxOptionHelper.ComboBoxOption>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public CustomComboBox()
    {
        InitializeComponent();
    }

    private void Expand_ComboBox(object sender, RoutedEventArgs e)
    {
        ComboBox.IsDropDownOpen = true;
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectionChanged?.Invoke(this, e);
    }

    private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
    {
        ComboBoxLostFocus?.Invoke(this, e);
    }
}
