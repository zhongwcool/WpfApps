using System.Windows;
using System.Windows.Threading;
using App19.SciChart.Controls;
using SciChart.Charting.Visuals;

namespace App19.SciChart;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;

        // Set this code once in App.xaml.cs or application startup
        SciChartSurface.SetRuntimeLicenseKey(
            "DvMW2NzzEE7gD/jgCd692V4xRASs3QcLOjj7rnLf8BybI2dBNAIMRyx+kba1/lhjXY1oMCaYV1tx7r6MuGtunSaTznVmV3sIUHwGAhpgRdyQ7PaXN6C99aLLICAByUXJ3TclTVE4j/2kLjLh8Px2Fq8N+MiqasoH1ahD3heUQGdvwz7fCzza+QFvj/vcq2S4JYgI0b/ADtQMM1SM3Qdd2M1a0GmPr5agEaWF2aspj8ji03gvEQMNPv5Hu2zOHBLfVZfX1aWlsJOF8xXihrBp2kvcJlQz4tMe0dPgFhfWNMIiVYzc9MBvY1o0NlM0BtWMtICm/H7xBURwFubgTb6pLkMdxDwBW2t0Lysj5/XHUa5aNtyaK1EI22sHf99wT7dcIH5Kh3JJqAAWJMyiHJx3JDN7ea+149sFnipyVFUcUIYVq43rvVmHp9mPjyqTUniTd/qugA2NFeSJtHaUHET7At7NPuhiCD9kzn4iP1XZclqC8p9fEm2PRfkMl9W1tJWnXvrsYT4Mn3uzqxScc7/ZZYOLcNrQv+xcjwzktXzGxg6b+QgFa0K8NUF2vlC//FDjiQ0=");
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var exceptionView = new ExceptionView(e.Exception)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        exceptionView.ShowDialog();

        e.Handled = true;
    }
}