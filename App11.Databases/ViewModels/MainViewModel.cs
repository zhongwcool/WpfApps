using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace App11.Databases.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }
}