using System.ComponentModel;
using App11.HIK.HikSdk;

namespace App11.HIK.Views;

public partial class CodeWindow
{
    public CodeWindow()
    {
        InitializeComponent();

        SizeChanged += (sender, args) =>
        {
            const double ratio = 16 / 10.0;
            Height = ActualWidth / ratio;
        };

        CameraController.Instance.CameraInit();
        CameraController.Instance.Display(grid1);
        CameraController.Instance.CameraLogin();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        CameraController.Instance.CameraLogout();
    }
}