using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using App18.Material.Dialogs;
using App18.Material.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace App18.Material.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(MainSnackbar.MessageQueue!);
        //开启全屏
        EntryMaximizedWindow();

        // 加载主题
        UpdateTheme();
        SystemEvents.UserPreferenceChanged += (_, args) =>
        {
            // 当事件是由于主题变化引起的
            if (args.Category == UserPreferenceCategory.General)
            {
                // 这里你可以写代码来处理主题变化，例如，重新加载样式或者资源
                UpdateTheme();
            }
        };

        var dispatcherTimer = new DispatcherTimer();
        // 当间隔时间过去时发生的事件
        dispatcherTimer.Tick += TickShowTime;
        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
        dispatcherTimer.Start();

        Task.Delay(2500).ContinueWith(_ =>
        {
            //note you can use the message queue from any thread, but just for the demo here we 
            //need to get the message queue from the snackbar, so need to be on the dispatcher
            Dispatcher.Invoke(() =>
            {
                MainSnackbar.MessageQueue?.Enqueue("Welcome to Material Design In XAML Toolkit");
            });
        });
    }

    private void UpdateTheme()
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        // 检查当前主题并应用
        switch (Theme.GetSystemTheme())
        {
            case BaseTheme.Light:
                theme.SetBaseTheme(BaseTheme.Light);
                DarkMode.IsChecked = false;
                break;
            case BaseTheme.Dark:
                theme.SetBaseTheme(BaseTheme.Dark);
                DarkMode.IsChecked = true;
                break;
        }

        paletteHelper.SetTheme(theme);
    }

    private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (NavDrawer.OpenMode is not DrawerHostOpenMode.Standard)
        {
            //until we had a StaysOpen flag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;

            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
        }
    }

    private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
    {
        ModifyTheme(DarkMode.IsChecked == true);
    }

    private static void ModifyTheme(bool isDarkTheme)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();

        theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);

        paletteHelper.SetTheme(theme);
    }

    private async void ButtonClose_OnClick(object sender, RoutedEventArgs e)
    {
        var content = new SingleTextDialog
        {
            Message = { Text = ((ButtonBase)sender).Content.ToString() }
        };

        await DialogHost.Show(content, "RootDialog", null, (o, args) =>
        {
            var ret = args.Parameter is true;
            if (ret) Close();
        });
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        switch (ActualWidth)
        {
            case <= 700:
                NavRail.Visibility = Visibility.Visible;
                NavDrawer.OpenMode = DrawerHostOpenMode.Modal;
                NavDrawer.IsLeftDrawerOpen = false;
                break;
            case > 700 and <= 1600:
                NavRail.Visibility = Visibility.Visible;
                NavDrawer.OpenMode = DrawerHostOpenMode.Modal;
                NavDrawer.IsLeftDrawerOpen = false;
                break;
            case > 1600:
                NavRail.Visibility = Visibility.Collapsed;
                NavDrawer.OpenMode = DrawerHostOpenMode.Standard;
                NavDrawer.IsLeftDrawerOpen = true;
                break;
        }
    }

    private void EntryMaximizedWindow()
    {
        WindowState = WindowState.Maximized;
        Topmost = false;
    }

    private void TickShowTime(object? sender, EventArgs e)
    {
        var week = CultureInfo.GetCultureInfo("zh-CN").DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
        //获得时分秒
        BlockPrefix.Text = DateTime.Now.ToString($"MM月dd日 {week} HH:mm:");
        BlockSecond.Text = DateTime.Now.ToString("ss");
    }

    private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();

        theme.SetPrimaryColor(Colors.Gold);
        theme.SetSecondaryColor(Colors.LimeGreen);

        var colorAdjustment = theme.ColorAdjustment ?? new ColorAdjustment();
        theme.ColorAdjustment = colorAdjustment;

        paletteHelper.SetTheme(theme);
    }
}