using System.Collections.ObjectModel;
using App22.Selected.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App22.Selected.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        CurrentStatus = StatusList.First();
    }

    private StatusItem _currentStatus = new StatusItem();

    public StatusItem CurrentStatus
    {
        get => _currentStatus;
        set => SetProperty(ref _currentStatus, value);
    }

    public ObservableCollection<StatusItem> StatusList { get; set; } =
    [
        new()
        {
            Status = "Locked",
            IconKey = "IconCircle"
        },
        new()
        {
            Status = "Unlocked",
            IconKey = "IconCircle"
        },
        new()
        {
            Status = "Locking",
            IconKey = "IconProt"
        },
        new()
        {
            Status = "Unknown",
            IconKey = "IconPoke"
        }
    ];
}