using CommunityToolkit.Mvvm.ComponentModel;

namespace App21.Skeleton.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
    }

    private bool _isButtonChecked;

    public bool IsButtonChecked
    {
        get => _isButtonChecked;
        set
        {
            SetProperty(ref _isButtonChecked, value);
            DataIsLoaded = IsButtonChecked;
        }
    }

    private bool _dataIsLoaded;

    public bool DataIsLoaded
    {
        get => _dataIsLoaded;
        set => SetProperty(ref _dataIsLoaded, value);
    }
}