using System.Windows;
using App11.HIK.HikSdk;

namespace App11.HIK.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        CameraController.Instance.CameraInit();
        CameraController.Instance.Display(grid1);
        CameraController.Instance.CameraLogin();
    }
}