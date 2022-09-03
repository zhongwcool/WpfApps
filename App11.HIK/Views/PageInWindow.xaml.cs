using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using App11.HIK.Models;
using App11.HIK.Views.Pages;

namespace App11.HIK.Views;

public partial class PageInWindow
{
    public PageInWindow()
    {
        InitializeComponent();

        // Create dummy data
        DoDummy();
    }

    private void DoDummy()
    {
        var task = JsNode.CreateDummy();
        task.ContinueWith(_ =>
        {
            var robots = task.Result;
            foreach (var robot in robots)
            {
                RobotList.Add(robot);
            }

            Dispatcher.Invoke(() => { ListViewDevice.ItemsSource = RobotList; });
        });
    }

    private ObservableCollection<JsNode> RobotList { get; set; } = new();

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