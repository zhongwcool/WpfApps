using System;
using System.Windows;
using System.Windows.Threading;
using App19.SciChart00.Controls;
using App19.SciChart00.Models;
using Mar.Cheese;
using SciChart.Charting.Visuals;

namespace App19.SciChart00;

public partial class App : Application
{
    public App()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;

        Prepare();
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

    private static void Prepare()
    {
        try
        {
            var model = JsonUtil.Load<Sci>(JSON_FILE);
            if (model == null) return;
            var license = model.License;

            // Set this code once in App.xaml.cs or application startup
            SciChartSurface.SetRuntimeLicenseKey(license);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private const string JSON_FILE = "scic.json";
}