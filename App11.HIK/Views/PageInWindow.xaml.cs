using System.Threading;
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
        if (sender is not ListView item) return;
        if (0 == item.SelectedIndex)
        {
            var de = item.SelectedItem as RobotModel;
            if (de is null) return;
            ShowCamera2(de.DevIp);
        }
    }

    private void ShowCamera2(string ip)
    {
        var page = new HikCameraPage(ip);
        HikControl.Content = page;
    }
}