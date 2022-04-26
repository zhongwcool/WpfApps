using App08.Metro.Concurrent;
using App08.Metro.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace App08.Metro.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        Notifications = new MtObservableCollection<NotiModel>();

        CommandConnect = new RelayCommand(() =>
        {
            Notifications.Insert(0, NotiModel.CreateDummy());
            while (Notifications.Count >= 10)
            {
                Notifications.RemoveAt(Notifications.Count - 1);
            }
        });
        CommandNetwork = new RelayCommand(() => { });
        CommandSettings = new RelayCommand(() => { });
        CommandAirplane = new RelayCommand(() => { });
        CommandLocate = new RelayCommand(() => { });
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    public IRelayCommand CommandConnect { get; }
    public IRelayCommand CommandNetwork { get; }
    public IRelayCommand CommandSettings { get; }
    public IRelayCommand CommandAirplane { get; }
    public IRelayCommand CommandLocate { get; }

    private bool _isInFocus;

    public bool IsInFocus
    {
        get => _isInFocus;
        set => SetProperty(ref _isInFocus, value);
    }

    private bool _isHotpot;

    public bool IsHotpot
    {
        get => _isHotpot;
        set => SetProperty(ref _isHotpot, value);
    }

    private bool _isNightMode;

    public bool IsNightMode
    {
        get => _isNightMode;
        set => SetProperty(ref _isNightMode, value);
    }

    private bool _isBluetooth;

    public bool IsBluetooth
    {
        get => _isBluetooth;
        set => SetProperty(ref _isBluetooth, value);
    }

    private bool _isProject;

    public bool IsProject
    {
        get => _isProject;
        set => SetProperty(ref _isProject, value);
    }

    public MtObservableCollection<NotiModel> Notifications { get; set; }
}