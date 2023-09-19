using System.Collections.Generic;
using System.Collections.ObjectModel;
using App08.DragDrop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App08.DragDrop.ViewModels;

public class TodoItemListingViewModel : ObservableObject
{
    private readonly ObservableCollection<TodoItem> _todoItems;

    public IEnumerable<TodoItem> TodoItems => _todoItems;

    private TodoItem _incomingTodoItem;

    public TodoItem IncomingTodoItem
    {
        get => _incomingTodoItem;
        set => SetProperty(ref _incomingTodoItem, value);
    }

    private TodoItem _removedTodoItem;

    public TodoItem RemovedTodoItem
    {
        get => _removedTodoItem;
        set => SetProperty(ref _removedTodoItem, value);
    }

    private TodoItem _insertedTodoItem;

    public TodoItem InsertedTodoItem
    {
        get => _insertedTodoItem;
        set => SetProperty(ref _insertedTodoItem, value);
    }

    private TodoItem _targetTodoItem;

    public TodoItem TargetTodoItem
    {
        get => _targetTodoItem;
        set => SetProperty(ref _targetTodoItem, value);
    }

    public IRelayCommand TodoItemReceivedCommand { get; }
    public IRelayCommand TodoItemRemovedCommand { get; }
    public IRelayCommand TodoItemInsertedCommand { get; }

    public TodoItemListingViewModel()
    {
        _todoItems = new ObservableCollection<TodoItem>();

        TodoItemReceivedCommand = new RelayCommand(TodoItemReceived);
        TodoItemRemovedCommand = new RelayCommand(TodoItemRemoved);
        TodoItemInsertedCommand = new RelayCommand(TodoItemInserted);
    }

    private void TodoItemInserted()
    {
        InsertTodoItem(InsertedTodoItem, TargetTodoItem);
    }

    private void TodoItemRemoved()
    {
        RemoveTodoItem(RemovedTodoItem);
    }

    private void TodoItemReceived()
    {
        AddTodoItem(IncomingTodoItem);
    }

    public void AddTodoItem(TodoItem item)
    {
        if (!_todoItems.Contains(item)) _todoItems.Add(item);
    }

    private void InsertTodoItem(TodoItem insertedTodoItem, TodoItem targetTodoItem)
    {
        if (insertedTodoItem == targetTodoItem) return;

        var oldIndex = _todoItems.IndexOf(insertedTodoItem);
        var nextIndex = _todoItems.IndexOf(targetTodoItem);

        if (oldIndex != -1 && nextIndex != -1) _todoItems.Move(oldIndex, nextIndex);
    }

    private void RemoveTodoItem(TodoItem item)
    {
        _todoItems.Remove(item);
    }
}