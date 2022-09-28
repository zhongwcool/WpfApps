using System.Windows;

namespace App15.Animated.Themes;

public partial class AnimativeDarkTheme
{
    private void CloseWindow_Event(object sender, RoutedEventArgs e)
    {
        if (e.Source == null) return;
        try
        {
            CloseWind(Window.GetWindow((FrameworkElement)e.Source));
        }
        catch
        {
            // ignored
        }
    }

    private void AutoMinimize_Event(object sender, RoutedEventArgs e)
    {
        if (e.Source == null) return;
        try
        {
            MaximizeRestore(Window.GetWindow((FrameworkElement)e.Source));
        }
        catch
        {
            // ignored
        }
    }

    private void Minimize_Event(object sender, RoutedEventArgs e)
    {
        if (e.Source == null) return;
        try
        {
            MinimizeWind(Window.GetWindow((FrameworkElement)e.Source));
        }
        catch
        {
            // ignored
        }
    }

    private static void CloseWind(Window window)
    {
        window.Close();
    }

    private static void MaximizeRestore(Window window)
    {
        switch (window.WindowState)
        {
            case WindowState.Maximized:
                window.WindowState = WindowState.Normal;
                break;
            case WindowState.Normal:
                window.WindowState = WindowState.Maximized;
                break;
        }
    }

    private static void MinimizeWind(Window window)
    {
        window.WindowState = WindowState.Minimized;
    }
}