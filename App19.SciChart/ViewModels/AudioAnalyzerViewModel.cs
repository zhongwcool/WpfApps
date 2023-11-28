using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using App19.SciChart00.Audio;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xaml.Behaviors.Core;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;

namespace App19.SciChart00.ViewModels;

public class AudioAnalyzerViewModel : ObservableObject
{
    public AudioAnalyzerViewModel()
    {
        _dispatcher = Dispatcher.CurrentDispatcher;

        StartCommand = new ActionCommand(OnExampleEnter);
        StopCommand = new ActionCommand(OnExampleExit);
    }

    private readonly Dispatcher _dispatcher;

    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    private IXyDataSeries<int, double> _dataSeries;

    public IXyDataSeries<int, double> DataSeries
    {
        get => _dataSeries;
        set => SetProperty(ref _dataSeries, value);
    }

    private void Update(object? sender, EventArgs e)
    {
        Update((AudioDataAnalyzer)sender);
    }

    private IDataSeries _uniformHeatmapDataSeries;

    public IDataSeries UniformHeatmapDataSeries
    {
        get => _uniformHeatmapDataSeries;
        set => SetProperty(ref _uniformHeatmapDataSeries, value);
    }

    private readonly AudioDeviceSource _audioDeviceSource = new();
    private AudioDeviceHandler _audioDeviceHandler;

    public ObservableCollection<AudioDeviceInfo> AvailableDevices => _audioDeviceSource.Devices;

    private string _selectedDeviceId;

    public string SelectedDeviceId
    {
        get => _selectedDeviceId;
        set
        {
            SetProperty(ref _selectedDeviceId, value);
            HandleInputDeviceChange(value);
            for (var index = 0; index < AvailableDevices.Count; index++)
            {
                var device = AvailableDevices[index];
                if (device.ID != value) continue;
                SelectedIndex = index;
                break;
            }
        }
    }

    private int _selectedIndex;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    private string FindDefaultDevice()
    {
        string deviceId = null;
        try
        {
            deviceId = _audioDeviceSource.DefaultDevice;
            if (deviceId == null) deviceId = AvailableDevices.FirstOrDefault()?.ID;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return deviceId;
    }

    private void Init()
    {
        // The actual process is handled in SelectedDeviceId setter
        SelectedDeviceId = FindDefaultDevice();
    }

    private AudioDataAnalyzer _audioDataAnalyzer;

    private void HandleInputDeviceChange(string newId)
    {
        _audioDeviceHandler?.Dispose();
        _audioDeviceHandler = null;
        if (_audioDataAnalyzer != null)
        {
            _audioDataAnalyzer.Update -= Update;
            _audioDataAnalyzer = null;
        }

        if (newId == null)
        {
            newId = FindDefaultDevice();

            if (newId == null)
            {
                MessageBox.Show("Unable to find capture device, please make sure that you have connected mic");
                return;
            }

            SelectedDeviceId = newId;
            return;
        }

        _audioDeviceHandler = _audioDeviceSource.CreateHandler(newId);
        _audioDataAnalyzer = new AudioDataAnalyzer(_audioDeviceHandler);
        _audioDataAnalyzer.Update += Update;
        InitDataStorage(_audioDataAnalyzer, _audioDeviceHandler);
        _audioDeviceHandler.Start();
    }

    private IXyDataSeries<int, double> _frequencyDataSeries;

    public IXyDataSeries<int, double> FrequencyDataSeries
    {
        get => _frequencyDataSeries;
        set => SetProperty(ref _frequencyDataSeries, value);
    }

    private void Update(AudioDataAnalyzer analyzer)
    {
        using (_dataSeries.SuspendUpdates())
        {
            _dataSeries.Clear();
            _dataSeries.Append(analyzer.PrimaryIndices, analyzer.Samples);
        }

        using (_frequencyDataSeries.SuspendUpdates())
        {
            _frequencyDataSeries.Clear();
            _frequencyDataSeries.Append(analyzer.FftIndices, analyzer.DbValues);
        }

        _uniformHeatmapDataSeries.InvalidateParentSurface(RangeMode.None);
    }

    private void InitDataStorage(AudioDataAnalyzer analyzer, AudioDeviceHandler handler)
    {
        UniformHeatmapDataSeries =
            new UniformHeatmapDataSeries<int, int, double>(analyzer.SpectrogramBuffer, 0, analyzer.FftFrequencySpacing,
                0, 1);

        var dataSeries = new XyDataSeries<int, double>(handler.BufferSize);
        dataSeries.Append(analyzer.PrimaryIndices, analyzer.Samples);
        DataSeries = dataSeries;

        var freqDataSeries = new XyDataSeries<int, double>(analyzer.FftDataPoints);
        freqDataSeries.Append(analyzer.FftIndices, analyzer.DbValues);
        FrequencyDataSeries = freqDataSeries;
    }

    // These methods are just used to do tidy up when switching between examples
    private void OnExampleExit()
    {
        _audioDeviceHandler?.Dispose();
        _dispatcher.ShutdownStarted -= DispatcherShutdownStarted;
    }

    private void OnExampleEnter()
    {
        // We must clean up the audio client (if any), otherwise it will block app shutdown
        _dispatcher.ShutdownStarted += DispatcherShutdownStarted;

        try
        {
            Init();
        }
        catch
        {
            MessageBox.Show("Unable to use default audio device");
        }
    }

    private void DispatcherShutdownStarted(object? sender, EventArgs e)
    {
        OnExampleExit();
    }
}