using System;
using System.Windows;
using App11.HIK.HikSdk;
using App11.HIK.Utils;
using MessageBox = System.Windows.Forms.MessageBox;

namespace App11.HIK.Views;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();

        // Init Hik SDK
        var init = CameraInit();
        if (!init)
        {
            Log.E("Hik SDK初始化失败！");
            MessageBox.Show("Hik SDK初始化失败！");
        }
        else
        {
            App.IsHikSdkPrepared = true;
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        // Clean hik sdk
        CHCNetSDK.NET_DVR_Cleanup();
    }

    private void ButtonDyna_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new CodeWindow();
        window.ShowDialog();
    }

    private void ButtonXaml_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new XamlWindow();
        window.ShowDialog();
    }

    private void ButtonPage_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new PageInWindow();
        window.ShowDialog();
    }

    private void ButtonToast_OnClick(object sender, RoutedEventArgs e)
    {
        AppUtil.ShowToast("我是这样的");
    }

    private void ButtonDuo_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new Duo2Window();
        window.ShowDialog();
    }

    private static bool CameraInit()
    {
        if (CHCNetSDK.NET_DVR_Init())
        {
            CHCNetSDK.NET_DVR_SetLogToFile(3, "02-SdkLog", true);
            return true;
        }

        MessageBox.Show("SDK初始化失败！");
        Log.D("SDK初始化失败！");
        return false;
    }

    private void ButtonDuo1_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new DuoWindow();
        window.ShowDialog();
    }
}