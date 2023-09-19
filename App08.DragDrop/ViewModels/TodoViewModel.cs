using CommunityToolkit.Mvvm.ComponentModel;

namespace App08.DragDrop.ViewModels;

public class TodoViewModel : ObservableObject
{
    public TodoItemListingViewModel InProgressTodoItemListingViewModel { get; }
    public TodoItemListingViewModel CompletedTodoItemListingViewModel { get; }

    public TodoViewModel(TodoItemListingViewModel inProgressTodoItemListingViewModel,
        TodoItemListingViewModel completedTodoItemListingViewModel)
    {
        InProgressTodoItemListingViewModel = inProgressTodoItemListingViewModel;
        CompletedTodoItemListingViewModel = completedTodoItemListingViewModel;
    }
}