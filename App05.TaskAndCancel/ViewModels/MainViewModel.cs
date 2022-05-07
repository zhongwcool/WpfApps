using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace App05.TaskAndCancel.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        CommandConnect = new RelayCommand(() => { });
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    public IRelayCommand CommandConnect { get; }

    private string _txtToday;

    public string TxtToday
    {
        get => _txtToday;
        set => SetProperty(ref _txtToday, value);
    }
}