using System.Windows;
using System.Windows.Controls;
using App11.HIK.Models;
using App11.HIK.ViewModels;
using App11.HIK.Views.Pages;

namespace App11.HIK.Views;

public partial class ByPageWindow
{
    public ByPageWindow()
    {
        InitializeComponent();
        DataContext = new ByPageViewModel();
    }

    private void MySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView { SelectedItem: JsNode de }) return;

        // Clear or remove current page
        HikControl.Children.Clear();
        if (_currentPage is HikCameraPage hik)
        {
            hik.Dispose();
        }

        // Add mew page
        ShowCamera2(de);
    }

    private UIElement _currentPage;

    private void ShowCamera2(JsNode robot)
    {
        _currentPage = new HikCameraPage(robot);
        HikControl.Children.Add(_currentPage);
    }
}