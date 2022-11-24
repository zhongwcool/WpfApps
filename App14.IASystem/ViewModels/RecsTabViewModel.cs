using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class RecsTabViewModel : ObservableObject
{
    private IaContext Context { get; }

    public ObservableCollection<RecWqm> WqmsCollection { get; set; }
    public ObservableCollection<Pool> PoolList { get; }

    private Pool _selectedPool = new();

    public Pool SelectedPool
    {
        get => _selectedPool;
        set
        {
            SetProperty(ref _selectedPool, value);
            SelectedDeviceIndex = 0;
        }
    }

    private Device _selectedDevice;

    public Device SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }

    private int _selectedPoolIndex = -1;

    public int SelectedPoolIndex
    {
        get => _selectedPoolIndex;
        set
        {
            SetProperty(ref _selectedPoolIndex, value);
            SelectedDeviceIndex = 0;
        }
    }

    private int _selectedDeviceIndex = 0;

    public int SelectedDeviceIndex
    {
        get => _selectedDeviceIndex;
        set => SetProperty(ref _selectedDeviceIndex, value);
    }

    public RecsTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.Pools.Load();
        Context.Devices.Load();
        Context.RecWqms.Load();
        // bind to the source
        WqmsCollection = Context.RecWqms.Local.ToObservableCollection();
        PoolList = Context.Pools.Local.ToObservableCollection();

        Task.Delay(2000).ContinueWith(_ => { SelectedPoolIndex = 0; });

        GenerateDataCommand = new RelayCommand<bool>(DoGenerateData, CanExecute_GenerateDataCommand);
    }

    private Device _selectedRecWqm = new();

    public Device SelectedRecWqm
    {
        get => _selectedRecWqm;
        set => SetProperty(ref _selectedRecWqm, value);
    }

    private BackgroundWorker _worker;

    public IRelayCommand<bool> GenerateDataCommand { get; }

    private void DoGenerateData(bool isChecked)
    {
        if (isChecked)
        {
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true; //允许取消
            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _worker.RunWorkerAsync(100);
        }
        else
        {
            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
                _worker.Dispose();
            }
        }
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        //如果用户取消了当前操作就关闭窗口。
        if (!e.Cancelled)
        {
            //计算结果信息：e.Result
            MessageBox.Show("用户取消:" + e.Result);
        }

        //计算已经结束，需要禁用取消按钮。
        //TODO DO SOMETHING

        //计算过程中的异常会被抓住，在这里可以进行处理。
        if (e.Error == null) return;
        var errorType = e.Error.GetType();
        switch (errorType.Name)
        {
            case "ArgumentNullException":
            case "MyException":
                //do something.
                break;
            default:
                //do something.
                break;
        }
    }

    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        CurrentProgress = e.ProgressPercentage;
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        var bgWorker = sender as BackgroundWorker;
        var max = 0;
        if (e.Argument != null)
        {
            max = (int)e.Argument;
        }

        var result = 0;
        for (var i = 0; i < max; i++)
        {
            var progressPercentage = Convert.ToInt32((double)i / max * 100);

            var rand = new Random();
            var t = rand.NextSingle() + rand.Next(0, 30);
            var p = rand.NextSingle() + rand.Next(5, 9);
            var ds = rand.NextSingle() + rand.Next(95, 101);
            var dr = rand.NextSingle() + rand.Next(90, 105);
            var rec = new RecWqm
            {
                Id = Guid.NewGuid(), HtTemp = t, HtPh = p, HtDosat = ds, HtDor = dr, Pool = SelectedPool,
                Device = SelectedDevice, TimeStamp = DateTime.Now
            };
            Context.Add(rec);
            Context.SaveChanges();

            bgWorker?.ReportProgress(progressPercentage);

            //在操作的过程中需要检查用户是否取消了当前的操作。
            if (bgWorker?.CancellationPending == true)
            {
                e.Cancel = true;
                break;
            }

            Thread.Sleep(100);
        }

        e.Result = result;
    }

    private double _currentProgress;

    public double CurrentProgress
    {
        get => _currentProgress;
        set => SetProperty(ref _currentProgress, value);
    }

    private bool CanExecute_GenerateDataCommand(bool isChecked)
    {
        return _worker is not { IsBusy: true };
    }
}