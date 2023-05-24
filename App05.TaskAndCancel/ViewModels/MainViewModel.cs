using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App05.TaskAndCancel.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel? _instance;
    private CancellationTokenSource _tokenSource = new();

    private MainViewModel()
    {
        CommandCancel = new RelayCommand(() =>
        {
            _tokenSource.Cancel();
            Devices.Add($"取消了Task.Wait: {DateTime.Now}\n");
        });

        CommandRenew = new RelayCommand(() =>
        {
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(Test);
        });

        Task.Factory.StartNew(Test);
    }

    public static MainViewModel? CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    private void Test()
    {
        Application.Current.Dispatcher.Invoke(() => Devices.Add($"开始: {DateTime.Now}\n"));
        Task.WaitAny(new[] { Task.Delay(10000, _tokenSource.Token) }, _tokenSource.Token);
        Application.Current.Dispatcher.Invoke(() => Devices.Add($"结束: {DateTime.Now}\n"));
    }

    public IRelayCommand CommandCancel { get; }
    public IRelayCommand CommandRenew { get; }

    public ObservableCollection<string> Devices { get; } = new();
}