using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using ModernWpf;
using Serilog;

namespace App11.HIK;

public partial class App : Application
{
    private readonly string _file = Path.Combine("00-Log", "log.txt");

    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:l}{NewLine}{Exception}")
            .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:l}{NewLine}{Exception}")
            .WriteTo.File(
                _file,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:l} {NewLine}{Exception}")
            .CreateLogger();
        Log.Fatal("Hello, Serilog!");

        DependencyPropertyDescriptor.FromProperty(ThemeManager.ApplicationThemeProperty, typeof(ThemeManager))
            .AddValueChanged(ThemeManager.Current, delegate { UpdateApplicationTheme(); });

        UpdateApplicationTheme();
    }

    #region ApplicationTheme

    public static string CurrentTheme { get; set; } = "Dark";

    private void UpdateApplicationTheme()
    {
        var theme1 = ThemeManager.Current.ApplicationTheme;
        var theme2 = ThemeManager.Current.ActualApplicationTheme;
        CurrentTheme = string.IsNullOrEmpty(theme1.ToString()) ? theme2.ToString() : theme1.ToString();
        Console.WriteLine();
    }

    #endregion

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        Log.CloseAndFlush();
    }

    public static bool IsHikSdkPrepared { get; set; } = false;
}