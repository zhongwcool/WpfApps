// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioDeviceHandler.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace App19.Audio;

internal class AudioDeviceHandler : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly CancellationToken _token;
    private readonly AutoResetEvent _processEvt = new(false);
    private readonly WasapiCapture _capture;
    private readonly MMDevice _device;
    private readonly WaveFormat _waveFormat;
    private readonly SampleReader _reader;
    private readonly double[] _input;

    public int SamplesPerSecond { get; }
    public int BufferSize { get; }

    public double[] Samples { get; }

    public event EventHandler DataReceived;

    public AudioDeviceHandler(MMDevice device)
    {
        _token = _cts.Token;
        _device = device;

        _waveFormat = device.AudioClient.MixFormat.AsStandardWaveFormat();

        SamplesPerSecond = _waveFormat.SampleRate;

        BufferSize = SamplesPerSecond * 10;

        _input = new double[BufferSize];
        Samples = new double[BufferSize];

        var capture = new WasapiCapture(device, false, 10) { WaveFormat = _waveFormat };
        capture.DataAvailable += DataAvailable;
        capture.RecordingStopped += RecordingStopped;

        _capture = capture;

        _reader = new SampleReader(_waveFormat);

        _ = Task.Run(ProcessData, _cts.Token);
    }

    public void Start()
    {
        _capture.StartRecording();
    }

    private void Stop()
    {
        _capture.StopRecording();
    }

    private void ProcessData()
    {
        while (!_token.IsCancellationRequested)
            if (_processEvt.WaitOne(100))
            {
                lock (_input)
                {
                    Array.Copy(_input, Samples, _input.Length);
                }

                DataReceived?.Invoke(this, EventArgs.Empty);
            }
    }

    private void RecordingStopped(object sender, StoppedEventArgs e)
    {
        if (_token.IsCancellationRequested)
        {
            _capture.Dispose();
            _device.Dispose();
        }
    }

    private void DataAvailable(object sender, WaveInEventArgs e)
    {
        if (e.BytesRecorded == 0) return;

        // We need to get off this thread ASAP to avoid losing frames
        lock (_input)
        {
            _reader.ReadSamples(e.Buffer, e.BytesRecorded, _input);
            _processEvt.Set();
        }
    }

    public void Dispose()
    {
        if (_capture.CaptureState == CaptureState.Stopped)
        {
            _capture.Dispose();
            _device.Dispose();
        }
        else
        {
            Stop();
        }

        _cts.Dispose();
    }
}