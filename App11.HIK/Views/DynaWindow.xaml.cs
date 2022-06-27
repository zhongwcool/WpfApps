using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using App11.HIK.Concurrent;
using App11.HIK.HikSdk;
using App11.HIK.Models;

namespace App11.HIK.Views;

public partial class DynaWindow : Window
{
    public DynaWindow()
    {
        InitializeComponent();

        RobotList.Insert(0, RobotModel.CreateDummy());
        Thread.Sleep(500);
        RobotList.Insert(0, RobotModel.CreateDummy());
        ListViewDevice.ItemsSource = RobotList;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        CameraController.Instance.CameraLogout();
    }

    private MtObservableCollection<RobotModel> RobotList { get; set; } = new();

    private void MySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView item) return;
        if (0 == item.SelectedIndex)
        {
            ShowCamera();
        }
    }

    private void ShowCamera()
    {
        CameraController.Instance.CameraInit();
        CameraController.Instance.Display(grid1);
        CameraController.Instance.CameraLogin();
    }
}