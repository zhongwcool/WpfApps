using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using App19.SciChart00.ViewModels;
using MaterialDesignThemes.Wpf;

namespace App19.SciChart00.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(MainSnackbar.MessageQueue!);
        ReduceBackground();

        Task.Delay(2500).ContinueWith(_ =>
        {
            //note you can use the message queue from any thread, but just for the demo here we 
            //need to get the message queue from the snackbar, so need to be on the dispatcher
            Dispatcher.Invoke(() => { MainSnackbar.MessageQueue?.Enqueue("Welcome to SciChart"); });
        });
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

    private void ReduceBackground()
    {
        // 获取当前按钮的背景色 Brush
        if (DrawerView.Background is not SolidColorBrush brush0) return;
        // 创建具有修改透明度的新 Brush
        var modifiedBackground = new SolidColorBrush(brush0.Color)
        {
            Opacity = 0.6 // 设置新的透明度
        };

        // 将按钮的背景色设置为修改后的 Brush
        DrawerView.Background = modifiedBackground;
        NavRail.Background = modifiedBackground;
    }
}