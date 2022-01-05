using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace WpfApp1.ViewModel;

public class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        CommandPlay = new RelayCommand(() =>
        {
            IsBusy = true;
            UpdateCommandStatus();
        }, () => !IsBusy);

        CommandStop = new RelayCommand(() =>
        {
            IsBusy = false;
            UpdateCommandStatus();
        }, () => IsBusy);
    }

    private bool _isBusy;

    private bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public IRelayCommand CommandPlay { get; }
    public IRelayCommand CommandStop { get; }

    private void UpdateCommandStatus()
    {
        CommandPlay.NotifyCanExecuteChanged();
        CommandStop.NotifyCanExecuteChanged();
    }
}