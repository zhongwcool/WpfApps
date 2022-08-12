using System;
using System.Globalization;
using System.Windows.Input;
using App04.MultiLang.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace App04.MultiLang.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        SendCommand = new RelayCommand(() =>
        {
            WeakReferenceMessenger.Default.Send(new Message
                { Num = 123, Str = DateTime.Now.ToString(CultureInfo.CurrentCulture) });
        });
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    private string _infoText = "风景秀丽";

    public string InfoText
    {
        get => _infoText;

        set => SetProperty(ref _infoText, value);
    }

    public ICommand SendCommand { get; }
}