using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace App01.VLC.Controls;

public sealed class ClickableTextBlock : Control
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(ClickableTextBlock),
            new PropertyMetadata(default(string)));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // Click事件
    public static readonly RoutedEvent ClickEvent =
        EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(RoutedEventHandler),
            typeof(ClickableTextBlock));

    // .NET事件包装器
    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    // 增加事件触发
    private void OnClick()
    {
        var newEventArgs = new RoutedEventArgs(ClickEvent);
        RaiseEvent(newEventArgs);
    }

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ClickableTextBlock),
            new PropertyMetadata(null)
        );

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(ClickableTextBlock),
            new PropertyMetadata(null));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    // 构造函数
    static ClickableTextBlock()
    {
        // 通过覆盖元数据以实现按键调用OnClick方法
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ClickableTextBlock),
            new FrameworkPropertyMetadata(typeof(ClickableTextBlock)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // 为整个控件添加点击事件处理程序
        MouseLeftButtonDown += (s, e) =>
        {
            OnClick(); // 触发点击事件

            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        };
    }
}