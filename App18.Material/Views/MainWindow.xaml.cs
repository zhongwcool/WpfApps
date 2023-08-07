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

namespace App18.Material.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(MainSnackbar.MessageQueue!);

        //开启全屏
        EntryMaximizedWindow();

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

    private void OnSelectedItemChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        MainScrollViewer.ScrollToHome();
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
        ModifyTheme(DarkModeToggleButton.IsChecked == true);
    }

    private static void ModifyTheme(bool isDarkTheme)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();

        theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);

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
        MainSnackbar.MessageQueue?.Enqueue("Function is under development.");
    }
}