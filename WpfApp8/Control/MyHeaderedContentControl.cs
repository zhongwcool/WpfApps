using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp8.Control;

public class MyHeaderedContentControl : HeaderedContentControl
{
    public static readonly DependencyProperty ClickCommandProperty
        = DependencyProperty.Register(
            "ClickCommand",
            typeof(ICommand),
            typeof(MyHeaderedContentControl),
            new PropertyMetadata(null, OnCommandPropertyChanged));

    public ICommand ClickCommand
    {
        get => (ICommand)GetValue(ClickCommandProperty);

        set => SetValue(ClickCommandProperty, value);
    }

    private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not MyHeaderedContentControl control) return;

        control.MouseLeftButtonDown -= OnControlLeftClick;
        control.MouseLeftButtonDown += OnControlLeftClick;
    }

    private static void OnControlLeftClick(object sender, MouseButtonEventArgs e)
    {
        var control = sender as MyHeaderedContentControl;
        if (control?.ClickCommand == null) return;

        var command = control.ClickCommand;

        var element = e.OriginalSource as FrameworkElement;
        var name = element?.Name;

        switch (name)
        {
            case null:
                return;
            case "PART_Title":
            case "PART_Back":
                if (command.CanExecute(null)) command.Execute(null);
                break;
        }

        e.Handled = true;
    }
}