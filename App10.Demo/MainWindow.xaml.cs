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
    readonly DispatcherTimer m_Timer1 = new();

    private double _mPercent = 0;
    private bool _mIsStart = false;

    public MainWindow()
    {
        InitializeComponent();

        m_Timer1.Interval = new TimeSpan(0, 0, 0, 0, 100);
        m_Timer1.Tick += M_Timer1_Tick;
    }

    private void M_Timer1_Tick(object sender, EventArgs e)
    {
        _mPercent += 0.01;
        if (_mPercent > 1)
        {
            _mPercent = 1;
            m_Timer1.Stop();
            _mIsStart = false;
            StartChange(_mIsStart);
        }

        CircleProgressBar.CurrentValue1 = _mPercent;
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
            m_Timer1.Stop();
            _mIsStart = false;
        }
        else
        {
            _mPercent = 0;
            m_Timer1.Start();
            _mIsStart = true;
        }

        StartChange(_mIsStart);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        m_Timer1.Stop();
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