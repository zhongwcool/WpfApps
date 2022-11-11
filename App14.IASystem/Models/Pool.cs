using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App14.IASystem.Models;

public class Pool : ObservableObject
{
    private Guid _poolId;

    public Guid PoolId
    {
        get => _poolId;
        set => SetProperty(ref _poolId, value);
    }

    private string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _note;

    public string Note
    {
        get => _note;
        set => SetProperty(ref _note, value);
    }

    private int _progress;

    public int Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    private DateTime? _addDate;

    public DateTime? AddDate
    {
        get => _addDate;
        set => SetProperty(ref _addDate, value);
    }

    private Farm _farm;

    public virtual Farm Farm
    {
        get => _farm;
        set => SetProperty(ref _farm, value);
    }

    private IList<Device> _devices;

    public virtual IList<Device> Devices
    {
        get => _devices;
        set => SetProperty(ref _devices, value);
    }
}