using System;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using WpfApp4.Models;

namespace WpfApp4.ViewModel;

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