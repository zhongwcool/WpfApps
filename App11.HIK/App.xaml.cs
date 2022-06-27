using System;
using System.ComponentModel;
using System.Windows;
using ModernWpf;

namespace App11.HIK;

public partial class App : Application
{
    public App()
    {
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
}