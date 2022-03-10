using System.Windows.Controls;

namespace WpfApp8.Views;

public partial class SidePanelRight : UserControl
{
    public SidePanelRight()
    {
        InitializeComponent();
        SizeChanged += (_, _) =>
        {
            var diff = ActualHeight - Control0.ActualHeight - 40;
            Control4.Height = diff < 0 ? 0 : diff;
        };
    }
}