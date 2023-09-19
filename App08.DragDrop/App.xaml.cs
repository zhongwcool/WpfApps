using System.Windows;
using App08.DragDrop.Models;
using App08.DragDrop.ViewModels;
using App08.DragDrop.Views;

namespace App08.DragDrop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var inProgressTodoItemListingViewModel = new TodoItemListingViewModel();
        inProgressTodoItemListingViewModel.AddTodoItem(new TodoItem("Go jogging"));
        inProgressTodoItemListingViewModel.AddTodoItem(new TodoItem("Walk the dog"));
        inProgressTodoItemListingViewModel.AddTodoItem(new TodoItem("Make videos"));

        var completedTodoItemListingViewModel = new TodoItemListingViewModel();
        completedTodoItemListingViewModel.AddTodoItem(new TodoItem("Take a shower"));
        completedTodoItemListingViewModel.AddTodoItem(new TodoItem("Eat breakfast"));

        var todoViewModel = new TodoViewModel(inProgressTodoItemListingViewModel, completedTodoItemListingViewModel);

        MainWindow = new MainWindow
        {
            DataContext = todoViewModel
        };
        MainWindow.Show();

        base.OnStartup(e);
    }
}