using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using WpfApp8.Models;

namespace WpfApp8.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        CommandConnect = new RelayCommand(() => { CntConnect++; });
        CommandNetwork = new RelayCommand(() => { CntNetwork++; });
        CommandSettings = new RelayCommand(() => { CntSettings++; });
        CommandAirplane = new RelayCommand(() => { CntAirplane++; });
        CommandLocate = new RelayCommand(() => { CntLocate++; });

        Notifications = new ObservableCollection<NotiModel>();
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    private uint _cntConnect;

    public uint CntConnect
    {
        get => _cntConnect;
        private set
        {
            Notifications.Insert(0, NotiModel.CreateDummy());
            SetProperty(ref _cntConnect, value);
        }
    }

    public IRelayCommand CommandConnect { get; }

    private uint _cntNetwork;

    public uint CntNetwork
    {
        get => _cntNetwork;
        set => SetProperty(ref _cntNetwork, value);
    }

    public IRelayCommand CommandNetwork { get; }

    private uint _cntSettings;

    public uint CntSettings
    {
        get => _cntSettings;
        set => SetProperty(ref _cntSettings, value);
    }

    public IRelayCommand CommandSettings { get; }

    private uint _cntAirplane;

    public uint CntAirplane
    {
        get => _cntAirplane;
        set => SetProperty(ref _cntAirplane, value);
    }

    public IRelayCommand CommandAirplane { get; }

    private uint _cntLocate;

    public uint CntLocate
    {
        get => _cntLocate;
        set => SetProperty(ref _cntLocate, value);
    }

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

    public ObservableCollection<NotiModel> Notifications { get; set; }
}