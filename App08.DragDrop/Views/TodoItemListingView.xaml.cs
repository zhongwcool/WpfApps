using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace App08.DragDrop.Views;

public partial class TodoItemListingView : UserControl
{
    public static readonly DependencyProperty IncomingTodoItemProperty =
        DependencyProperty.Register(nameof(IncomingTodoItem), typeof(object), typeof(TodoItemListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object IncomingTodoItem
    {
        get => GetValue(IncomingTodoItemProperty);
        set => SetValue(IncomingTodoItemProperty, value);
    }

    public static readonly DependencyProperty RemovedTodoItemProperty =
        DependencyProperty.Register(nameof(RemovedTodoItem), typeof(object), typeof(TodoItemListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object RemovedTodoItem
    {
        get => GetValue(RemovedTodoItemProperty);
        set => SetValue(RemovedTodoItemProperty, value);
    }

    public static readonly DependencyProperty TodoItemDropCommandProperty =
        DependencyProperty.Register(nameof(TodoItemDropCommand), typeof(ICommand), typeof(TodoItemListingView),
            new PropertyMetadata(null));

    public ICommand TodoItemDropCommand
    {
        get => (ICommand)GetValue(TodoItemDropCommandProperty);
        set => SetValue(TodoItemDropCommandProperty, value);
    }

    public static readonly DependencyProperty TodoItemRemovedCommandProperty =
        DependencyProperty.Register(nameof(TodoItemRemovedCommand), typeof(ICommand), typeof(TodoItemListingView),
            new PropertyMetadata(null));

    public ICommand TodoItemRemovedCommand
    {
        get => (ICommand)GetValue(TodoItemRemovedCommandProperty);
        set => SetValue(TodoItemRemovedCommandProperty, value);
    }

    public static readonly DependencyProperty TodoItemInsertedCommandProperty =
        DependencyProperty.Register(nameof(TodoItemInsertedCommand), typeof(ICommand), typeof(TodoItemListingView),
            new PropertyMetadata(null));

    public ICommand TodoItemInsertedCommand
    {
        get => (ICommand)GetValue(TodoItemInsertedCommandProperty);
        set => SetValue(TodoItemInsertedCommandProperty, value);
    }

    public static readonly DependencyProperty InsertedTodoItemProperty =
        DependencyProperty.Register(nameof(InsertedTodoItem), typeof(object), typeof(TodoItemListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object InsertedTodoItem
    {
        get => GetValue(InsertedTodoItemProperty);
        set => SetValue(InsertedTodoItemProperty, value);
    }

    public static readonly DependencyProperty TargetTodoItemProperty =
        DependencyProperty.Register(nameof(TargetTodoItem), typeof(object), typeof(TodoItemListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object TargetTodoItem
    {
        get => GetValue(TargetTodoItemProperty);
        set => SetValue(TargetTodoItemProperty, value);
    }

    public TodoItemListingView()
    {
        InitializeComponent();
    }

    private void TodoItem_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || sender is not FrameworkElement frameworkElement) return;
        var todoItem = frameworkElement.DataContext;

        var dragDropResult = System.Windows.DragDrop.DoDragDrop(frameworkElement,
            new DataObject(DataFormats.Serializable, todoItem),
            DragDropEffects.Move);

        if (dragDropResult == DragDropEffects.None) AddTodoItem(todoItem);
    }

    private void TodoItem_DragOver(object sender, DragEventArgs e)
    {
        if (!(TodoItemInsertedCommand?.CanExecute(null) ?? false)) return;
        if (sender is not FrameworkElement element) return;
        TargetTodoItem = element.DataContext;
        InsertedTodoItem = e.Data.GetData(DataFormats.Serializable);

        TodoItemInsertedCommand?.Execute(null);
    }

    private void TodoItemList_DragOver(object sender, DragEventArgs e)
    {
        var todoItem = e.Data.GetData(DataFormats.Serializable);
        AddTodoItem(todoItem);
    }

    private void AddTodoItem(object todoItem)
    {
        if (!(TodoItemDropCommand?.CanExecute(null) ?? false)) return;
        IncomingTodoItem = todoItem;
        TodoItemDropCommand?.Execute(null);
    }

    private void TodoItemList_DragLeave(object sender, DragEventArgs e)
    {
        var result = VisualTreeHelper.HitTest(lvItems, e.GetPosition(lvItems));

        if (result != null) return;
        if (!(TodoItemRemovedCommand?.CanExecute(null) ?? false)) return;
        RemovedTodoItem = e.Data.GetData(DataFormats.Serializable);
        TodoItemRemovedCommand?.Execute(null);
    }
}