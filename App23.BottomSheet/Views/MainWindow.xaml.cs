﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace App23.BottomSheet.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // 加载主题
        UpdateTheme();
        // 监听系统主题变化
        SystemEvents.UserPreferenceChanged += (_, args) =>
        {
            // 当事件是由于主题变化引起的
            if (args.Category == UserPreferenceCategory.General)
                // 这里你可以写代码来处理主题变化，例如，重新加载样式或者资源
                UpdateTheme();
        };

        // Mosaic.Draw(MosaicRectangle);
    }

    private void ShowBottomSheet_Click(object sender, RoutedEventArgs e)
    {
        // Show the Overlay with animation
        Overlay.Visibility = Visibility.Visible;
        var showOverlayStoryboard = Resources["ShowOverlayStoryboard"] as Storyboard;
        showOverlayStoryboard?.Begin();

        // Animate the Bottom Sheet to slide up
        var animation = new DoubleAnimationUsingKeyFrames();
        animation.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
            new KeySpline(0.2, 0.8, 0.3, 1.0)));
        BottomSheet.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);
    }

    private void HideBottomSheet_Click(object sender, RoutedEventArgs e)
    {
        HideBottomSheet();
    }

    private void Overlay_MouseDown(object sender, RoutedEventArgs e)
    {
        HideBottomSheet();
    }

    private void HideBottomSheet()
    {
        // Animate the Bottom Sheet to slide down
        var animation = new DoubleAnimationUsingKeyFrames();
        animation.KeyFrames.Add(new SplineDoubleKeyFrame(200, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
            new KeySpline(0.8, 0.0, 0.9, 0.2)));

        animation.Completed += (s, e) =>
        {
            // Hide the Overlay with animation
            if (Resources["HideOverlayStoryboard"] is not Storyboard hideOverlayStoryboard) return;
            hideOverlayStoryboard.Completed += (s2, e2) => Overlay.Visibility = Visibility.Collapsed;
            hideOverlayStoryboard.Begin();
        };

        BottomSheet.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);
    }

    private static void UpdateTheme()
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        // 检查当前主题并应用
        switch (Theme.GetSystemTheme())
        {
            case BaseTheme.Light:
                theme.SetBaseTheme(BaseTheme.Light);
                break;
            case BaseTheme.Dark:
                theme.SetBaseTheme(BaseTheme.Dark);
                break;
        }

        paletteHelper.SetTheme(theme);
    }
}