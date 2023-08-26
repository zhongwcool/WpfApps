﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class RecsTabViewModel : ObservableObject
{
    private readonly IaContext _context;

    public RecsTabViewModel()
    {
        _context = new IaContext();
        // load the entities into EF Core
        _context.Pools.Load();
        _context.Devices.Load();
        _context.RecWqms.Load();
        // bind to the source
        Pools = new ObservableCollection<Pool>(_context.Pools.OrderBy(pool => pool.Name).ToList());

        Task.Delay(1500).ContinueWith(_ => { SelectedPoolIndex = 0; });

        DataCommand = new RelayCommand(DoGentData, CanExecute_DataCommand);
        StopCommand = new RelayCommand(DoStopData, CanExecute_StopCommand);
        ClearCommand = new RelayCommand(DoClearData);

        HtTempSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = HtTempValues,
                Fill = null
            }
        };

        PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(SelectedDevice):
                {
                    LoadRecsAndDraw();
                }
                    break;
            }
        };
    }

    private void LoadRecsAndDraw()
    {
        RecWqms.Clear();
        var query = _context.RecWqms.Local
            .Where(rec =>
                string.Equals(rec.Device.SerialNum, SelectedDevice.SerialNum, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.TimeStamp)
            .ToList();
        foreach (var recWqm in query) RecWqms.Add(recWqm);

        HtTempValues.Clear();
        var query2 = _context.RecWqms.Local
            .Where(rec =>
                string.Equals(rec.Device.SerialNum, SelectedDevice.SerialNum, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.TimeStamp)
            .Select(rec => new ObservableValue(rec.HtTemp))
            .Take(40)
            .ToList();
        foreach (var ht in query2) HtTempValues.Add(ht);
    }

    private ObservableCollection<ObservableValue> HtTempValues { get; } = new();

    public ObservableCollection<ISeries> HtTempSeries { get; set; }

    public ObservableCollection<RecWqm> RecWqms { get; set; } = new();
    public ObservableCollection<Pool> Pools { get; }

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

    private Device _selectedDevice = new();

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

    private bool _isBusy;

    private bool IsBusy
    {
        get => _isBusy;
        set
        {
            SetProperty(ref _isBusy, value);
            DataCommand.NotifyCanExecuteChanged();
            StopCommand.NotifyCanExecuteChanged();
        }
    }

    private Device _selectedRecWqm = new();

    public Device SelectedRecWqm
    {
        get => _selectedRecWqm;
        set => SetProperty(ref _selectedRecWqm, value);
    }

    public IRelayCommand ClearCommand { get; }

    private void DoClearData()
    {
        RecWqms.Clear();
        _context.RecWqms.Local.Clear();
        _context.SaveChanges();
    }

    #region 生产模拟数据

    public IRelayCommand StopCommand { get; }

    private void DoStopData()
    {
        _worker.CancelAsync();
        _worker.Dispose();
        IsBusy = false;
    }

    private bool CanExecute_StopCommand()
    {
        return IsBusy;
    }

    public IRelayCommand DataCommand { get; }

    private bool CanExecute_DataCommand()
    {
        return IsBusy is not true;
    }

    private void DoGentData()
    {
        _worker = new BackgroundWorker();
        _worker.WorkerReportsProgress = true;
        _worker.WorkerSupportsCancellation = true; //允许取消
        _worker.DoWork += Worker_DoWork;
        _worker.ProgressChanged += Worker_ProgressChanged;
        _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        _worker.RunWorkerAsync(100);

        IsBusy = true;
    }

    private BackgroundWorker _worker;

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        // 保存数据
        _context.SaveChanges();

        //如果用户取消了当前操作就关闭窗口。
        if (!e.Cancelled)
        {
            //计算结果信息：e.Result
            MessageBox.Show($"生产结束:{e.Result}%");
        }

        //计算已经结束，需要禁用取消按钮。
        IsBusy = false;

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

        var progressPercentage = 0;
        var adt = DateTime.Now;

        for (var i = 1; i <= max; i++)
        {
            progressPercentage = Convert.ToInt32((double)i / max * 100);

            var rand = new Random();
            var t = (float)Math.Round(rand.NextSingle() + rand.Next(0, 30), 2);
            var p = (float)Math.Round(rand.NextSingle() + rand.Next(5, 9), 2);
            var ds = (float)Math.Round(rand.NextSingle() + rand.Next(95, 101), 2);
            var dr = (float)Math.Round(rand.NextSingle() + rand.Next(90, 105), 2);
            var dt = adt.AddSeconds(i);

            var rec = new RecWqm
            {
                Id = Guid.NewGuid(), HtTemp = t, HtPh = p, HtDosat = ds, HtDor = dr, Pool = SelectedPool,
                Device = SelectedDevice, TimeStamp = dt
            };
            Application.Current.Dispatcher.Invoke(() =>
            {
                RecWqms.Add(rec);
                _context.RecWqms.Add(rec);
            });

            bgWorker?.ReportProgress(progressPercentage);

            //在操作的过程中需要检查用户是否取消了当前的操作。
            if (bgWorker?.CancellationPending == true)
            {
                e.Cancel = true;
                break;
            }

            Thread.Sleep(100);
        }

        e.Result = progressPercentage;
    }

    private double _currentProgress;

    public double CurrentProgress
    {
        get => _currentProgress;
        set => SetProperty(ref _currentProgress, value);
    }

    #endregion
}