using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using App10.Demo.Helper;
using ModernWpf;

namespace App10.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer _mTimer = new();

    private int _mPercent = 0;
    private bool _mIsStart = false;

    public MainWindow()
    {
        InitializeComponent();

        _mTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        _mTimer.Tick += MTimerTick;
    }

    private void MTimerTick(object sender, EventArgs e)
    {
        _mPercent += 1;
        if (_mPercent > 100)
        {
            _mPercent = 100;
            _mTimer.Stop();
            _mIsStart = false;
            StartChange(_mIsStart);
        }

        CircleProgressBar.CurrentValue = _mPercent;
    }

    /// <summary>
    /// UI变化
    /// </summary>
    /// <param name="bState"></param>
    private void StartChange(bool bState)
    {
        if (bState)
            BtnStart.Content = "停止";
        else
            BtnStart.Content = "开始";
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (_mIsStart)
        {
            _mTimer.Stop();
            _mIsStart = false;
        }
        else
        {
            _mPercent = 0;
            _mTimer.Start();
            _mIsStart = true;
        }

        StartChange(_mIsStart);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        _mTimer.Stop();
    }

    private void OnThemeButtonClick(object sender, RoutedEventArgs e)
    {
        DispatcherHelper.RunOnMainThread(() =>
        {
            if (ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
        });
    }
}