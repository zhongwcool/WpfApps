using System.ComponentModel;
using App11.HIK.HikSdk;

namespace App11.HIK.Views;

public partial class CodeOnlyWindow
{
    private CameraController ctrl0, ctrl1, ctrl2, ctrl3;

    public CodeOnlyWindow()
    {
        InitializeComponent();

        ctrl0 = new CameraController("192.168.77.102");
        ctrl0.Display(grid0);
        ctrl0.CameraLogin();

        ctrl1 = new CameraController("192.168.77.106");
        ctrl1.Display(grid1);
        ctrl1.CameraLogin();

        ctrl2 = new CameraController("192.168.77.107");
        ctrl2.Display(grid2);
        ctrl2.CameraLogin();

        ctrl3 = new CameraController("192.168.77.106");
        ctrl3.Display(grid3);
        ctrl3.CameraLogin();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        ctrl0?.CameraLogout();
        ctrl1?.CameraLogout();
        ctrl2?.CameraLogout();
        ctrl3?.CameraLogout();
    }
}