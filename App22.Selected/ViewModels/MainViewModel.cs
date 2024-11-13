using System.Collections.ObjectModel;
using App22.Selected.Enums;
using App22.Selected.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App22.Selected.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        CurrentStatus = StatusList.First();
    }

    private StatusItem _currentStatus = new();

    public StatusItem CurrentStatus
    {
        get => _currentStatus;
        set => SetProperty(ref _currentStatus, value);
    }

    public ObservableCollection<StatusItem> StatusList { get; set; } =
    [
        new()
        {
            Status = LockStatus.Locked,
            IconKey = "IconCherry"
        },
        new()
        {
            Status = LockStatus.Unlocked,
            IconKey = "IconGrape"
        },
        new()
        {
            Status = LockStatus.Locking,
            IconKey = "IconCheese"
        },
        new()
        {
            Status = LockStatus.Unknown,
            IconKey = "IconLemon"
        }
    ];
}