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
        MyHeaderedContentControl control = d as MyHeaderedContentControl;
        if (control == null) return;

        control.MouseLeftButtonDown -= OnControlLeftClick;
        control.MouseLeftButtonDown += OnControlLeftClick;
    }

    private static void OnControlLeftClick(object sender, MouseButtonEventArgs e)
    {
        MyHeaderedContentControl control = sender as MyHeaderedContentControl;
        if (control == null || control.ClickCommand == null) return;

        ICommand command = control.ClickCommand;

        if (command.CanExecute(null))
            command.Execute(null);

        e.Handled = true;
    }
}