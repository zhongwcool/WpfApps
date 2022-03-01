using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WpfApp8.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        CommandConnect = new RelayCommand(() => { });
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

    public ICommand CommandConnect { get; }
    public ICommand CommandNetwork { get; }
    public ICommand CommandSettings { get; }
    public ICommand CommandAirplane { get; }
    public ICommand CommandLocate { get; }

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
}