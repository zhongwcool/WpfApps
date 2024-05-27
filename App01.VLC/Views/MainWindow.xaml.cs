using System.Windows;
using System.Windows.Input;
using App01.VLC.Controls;
using App01.VLC.Dialogs;
using App01.VLC.Models;
using App01.VLC.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace App01.VLC.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();

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

        Loaded += async (_, _) =>
        {
            if (DataContext is MainViewModel vm) await vm.LoadDataAsync(true);
        };
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

    private UIElement _vlcview;

    private void OnItemClick(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is not Channel channel) return;
        _vlcview = new VlcView
        {
            DataContext = channel
        };
        ViewGrid.Children.Add(_vlcview);
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        //按Esc键移除VlcView
        if (e.Key != Key.Escape) return;
        ViewGrid.Children.Remove(_vlcview);
    }

    private async void ButtonSource_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        var content = new InputDialog()
        {
            DataContext = new InputDialogViewModel()
        };

        await DialogHost.Show(content, "DialogRoot", null, (_, args) =>
        {
            var ret = args.Parameter is true;
            // 获得Dialog输入结果
            if (ret)
            {
                if (content.DataContext is not InputDialogViewModel vm0) return;
                vm.UpdateSites(vm0.Site);
            }
        });
    }
}