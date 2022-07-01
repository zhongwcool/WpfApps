using System.Threading;
using System.Windows;
using System.Windows.Controls;
using App11.HIK.Concurrent;
using App11.HIK.Models;
using App11.HIK.Views.Pages;

namespace App11.HIK.Views;

public partial class PageInWindow
{
    public PageInWindow()
    {
        InitializeComponent();

        // Create dummy data
        RobotList.Insert(0, RobotModel.CreateDummy());
        Thread.Sleep(500);
        RobotList.Insert(0, RobotModel.CreateDummy());
        ListViewDevice.ItemsSource = RobotList;
    }

    private MtObservableCollection<RobotModel> RobotList { get; set; } = new();

    private void MySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView { SelectedItem: RobotModel de }) return;

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

    private void ShowCamera2(RobotModel robot)
    {
        _currentPage = new HikCameraPage(robot);
        HikControl.Children.Add(_currentPage);
    }
}