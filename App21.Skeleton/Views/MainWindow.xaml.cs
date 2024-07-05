using System.Windows;
using App21.Skeleton.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace App21.Skeleton.Views;

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
            if (DataContext is MainViewModel vm) await vm.LoadDataAsync();
        };
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
                break;
            case BaseTheme.Dark:
                theme.SetBaseTheme(BaseTheme.Dark);
                break;
        }

        paletteHelper.SetTheme(theme);
    }
}