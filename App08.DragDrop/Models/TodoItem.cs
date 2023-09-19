using CommunityToolkit.Mvvm.ComponentModel;

namespace App08.DragDrop.Models;

public class TodoItem : ObservableObject
{
    private string _description;

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public TodoItem(string description)
    {
        Description = description;
    }
}