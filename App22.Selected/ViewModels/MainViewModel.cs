using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App22.Selected.ViewModels;

public class MainViewModel : ObservableObject
{
    private string _currentStatus = "Locked";

    public string CurrentStatus
    {
        get => _currentStatus;
        set => SetProperty(ref _currentStatus, value);
    }

    public ObservableCollection<string> StatusList { get; set; } =
    [
        "Locked",
        "Unlocked"
    ];
}