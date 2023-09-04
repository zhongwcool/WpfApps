using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class DevicesTabViewModel : ObservableValidator, IDisposable
{
    private readonly IaContext _context;

    public DevicesTabViewModel()
    {
        DeleteDeviceCommand = new RelayCommand(DoDeleteDevice, CanExecute_DeleteDeviceCommand);
        SaveCommand = new RelayCommand(DoSave);

        PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(SelectedDevice):
                {
                    DeleteDeviceCommand.NotifyCanExecuteChanged();
                    if (null != SelectedDevice) SelectedValue = SelectedDevice.Pool;
                }
                    break;
                case nameof(SelectedValue):
                {
                    if (null != SelectedDevice)
                    {
                        SelectedDevice.Pool = SelectedValue;
                        _context?.Devices.Update(SelectedDevice);
                    }
                }
                    break;
            }
        };

        _context = new IaContext();
        // load the entities into EF Core
        _context.Devices.Load();
        // bind to the source
        OwnerList = _context.Pools.Select(pool => new Owner { Pool = pool, Description = pool.Name }).ToList();
        OwnerList.Insert(0, new Owner { Pool = null, Description = "None" });

        Devices = new ObservableCollection<Device>(_context.Devices.OrderBy(device => device.NodeName).ToList());
        SelectedDevice = Devices.FirstOrDefault();
    }

    public IRelayCommand DeleteDeviceCommand { get; }

    private void DoDeleteDevice()
    {
        _context.Devices.Remove(SelectedDevice);
        Devices.Remove(SelectedDevice);
    }

    private bool CanExecute_DeleteDeviceCommand()
    {
        return SelectedDevice != null;
    }

    public IRelayCommand SaveCommand { get; }

    private void DoSave()
    {
        ValidateAllProperties();
        if (!HasErrors) _context.SaveChanges();
    }

    public ObservableCollection<Device> Devices { get; set; }

    private Device _selectedDevice = new();

    public Device SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }

    private Pool _selectedValue;

    public Pool SelectedValue
    {
        get => _selectedValue;
        set => SetProperty(ref _selectedValue, value);
    }

    public List<Owner> OwnerList { get; }

    private void ReleaseUnmanagedResources()
    {
        _context.Dispose();
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing) _context?.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DevicesTabViewModel()
    {
        Dispose(false);
    }
}