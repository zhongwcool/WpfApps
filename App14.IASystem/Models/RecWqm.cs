using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.Models;

public class RecWqm : ObservableObject
{
    [Key] public Guid Id { get; set; }

    private float _htTemp;

    [Precision(5, 2)]
    public float HtTemp
    {
        get => _htTemp;
        set => SetProperty(ref _htTemp, value);
    }

    private float _htPh;

    [Precision(5, 2)]
    public float HtPh
    {
        get => _htPh;
        set => SetProperty(ref _htPh, value);
    }

    private float _htDosat;

    [Precision(5, 2)]
    public float HtDosat
    {
        get => _htDosat;
        set => SetProperty(ref _htDosat, value);
    }

    private float _htDor;

    [Precision(5, 2)]
    public float HtDor
    {
        get => _htDor;
        set => SetProperty(ref _htDor, value);
    }

    private DateTime? _timeStamp;

    public DateTime? TimeStamp
    {
        get => _timeStamp;
        set => SetProperty(ref _timeStamp, value);
    }

    private Pool _pool;

    public virtual Pool Pool
    {
        get => _pool;
        set => SetProperty(ref _pool, value);
    }

    private Device _device;

    public virtual Device Device
    {
        get => _device;
        set => SetProperty(ref _device, value);
    }
}