using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WpfApp8.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        CommandConnect = new RelayCommand(() => { TxtConnect = "Connect: " + _cntConnect++; });
        CommandNetwork = new RelayCommand(() => { TxtNetwork = "Network: " + _cntNetwork++; });
        CommandSettings = new RelayCommand(() => { TxtSettings = "Settings: " + _cntSettings++; });
        CommandAirplane = new RelayCommand(() => { TxtAirplane = "Airplane: " + _cntAirplane++; });
        CommandLocate = new RelayCommand(() => { TxtLocate = "Locate: " + _cntLocate++; });
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    private uint _cntConnect;
    private string _txtConnect;

    public string TxtConnect
    {
        get => _txtConnect;
        set => SetProperty(ref _txtConnect, value);
    }

    public IRelayCommand CommandConnect { get; }

    private uint _cntNetwork;
    private string _txtNetwork;

    public string TxtNetwork
    {
        get => _txtNetwork;
        set => SetProperty(ref _txtNetwork, value);
    }

    public IRelayCommand CommandNetwork { get; }

    private uint _cntSettings;
    private string _txtSettings;

    public string TxtSettings
    {
        get => _txtSettings;
        set => SetProperty(ref _txtSettings, value);
    }

    public IRelayCommand CommandSettings { get; }

    private uint _cntAirplane;
    private string _txtAirplane;

    public string TxtAirplane
    {
        get => _txtAirplane;
        set => SetProperty(ref _txtAirplane, value);
    }

    public IRelayCommand CommandAirplane { get; }

    private uint _cntLocate;
    private string _txtLocate;

    public string TxtLocate
    {
        get => _txtLocate;
        set => SetProperty(ref _txtLocate, value);
    }

    public IRelayCommand CommandLocate { get; }

    private string _textInFocus;

    public string TextInFocus
    {
        get => _textInFocus;
        set => SetProperty(ref _textInFocus, value);
    }

    private bool _isInFocus;

    public bool IsInFocus
    {
        get => _isInFocus;
        set
        {
            SetProperty(ref _isInFocus, value);
            TextInFocus = "焦点:" + value;
        }
    }

    private string _textHotpot;

    public string TextHotpot
    {
        get => _textHotpot;
        set => SetProperty(ref _textHotpot, value);
    }

    private bool _isHotpot;

    public bool IsHotpot
    {
        get => _isHotpot;
        set
        {
            SetProperty(ref _isHotpot, value);
            TextHotpot = "移动热点:" + value;
        }
    }

    private string _textNightMode;

    public string TextNightMode
    {
        get => _textNightMode;
        set => SetProperty(ref _textNightMode, value);
    }

    private bool _isNightMode;

    public bool IsNightMode
    {
        get => _isNightMode;
        set
        {
            SetProperty(ref _isNightMode, value);
            TextNightMode = "夜间模式:" + value;
        }
    }

    private string _textBluetooth;

    public string TextBluetooth
    {
        get => _textBluetooth;
        set => SetProperty(ref _textBluetooth, value);
    }

    private bool _isBluetooth;

    public bool IsBluetooth
    {
        get => _isBluetooth;
        set
        {
            SetProperty(ref _isBluetooth, value);
            TextBluetooth = "蓝牙:" + value;
        }
    }

    private string _textProject;

    public string TextProject
    {
        get => _textProject;
        set => SetProperty(ref _textProject, value);
    }

    private bool _isProject;

    public bool IsProject
    {
        get => _isProject;
        set
        {
            SetProperty(ref _isProject, value);
            TextProject = "投射:" + value;
        }
    }
}