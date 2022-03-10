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
        Notifications.Add(new NotiModel()
        {
            Title = "20分钟详解 - 俄罗斯经济到底是怎么回事儿？",
            ImageLocation = "https://img-blog.csdnimg.cn/20190323161418695.png",
            Description =
                "久等了，更新来啦！小Lin连夜肝稿，来解析一下错综复杂的俄罗斯经济到底是怎么回事儿~\n我的创业公司：Offer帮 - 求职学习平台\nhttps://offerbang.io?wpm=2.3.17.2",
            Date = "Mar 10, 2022"
        });
        Notifications.Add(new NotiModel()
        {
            Title = "WPF C# Professional Modern Flat UI Tutorial",
            ImageLocation = "https://img-blog.csdnimg.cn/20190323161159133.png",
            Description =
                "WPF C# Professional Modern Flat UI Tutorial this tutorial will show you how to create a flat modern ui  with a flat design using WPF and C# this goes really fast and it's stunning and beautiful and professional and works well for 2021 and 2022. This is a tutorial and the source code and the project files will be available for download. A simple, minimalistic futuristic look. \nPatreon: https://www.patreon.com/payloads ",
            Date = "Apr 4, 2021"
        });
        Notifications.Add(new NotiModel()
        {
            Title = "华为 MatePad Paper 深度体验 ：这就是墨水屏天花板？",
            ImageLocation = "https://img-blog.csdnimg.cn/20190323161242873.png",
            Description = "这，是买前生产力买后爱奇艺的平板，而这，是海鲜市场里被评为十大年度无用产品的墨水屏电纸书。啪的一下很快啊，合二为一了，能改变吃灰的命运吗？",
            Date = "Mar 10, 2022"
        });
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
        set => SetProperty(ref _cntConnect, value);
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