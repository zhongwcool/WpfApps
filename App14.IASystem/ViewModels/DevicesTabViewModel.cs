using System.Collections.ObjectModel;
using System.Linq;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class DevicesTabViewModel : ObservableValidator
{
    private readonly IaContext _context;

    public DevicesTabViewModel(IaContext context)
    {
        _context = context;

        DeleteDeviceCommand = new RelayCommand(DoDeleteDevice, CanExecute_DeleteDeviceCommand);
        SaveCommand = new RelayCommand(DoSave);

        PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(SelectedDevice):
                {
                    DeleteDeviceCommand.NotifyCanExecuteChanged();
                    SelectedPool = SelectedDevice?.Pool;
                }
                    break;
                case nameof(SelectedPool):
                {
                    SelectedDevice.Pool = SelectedPool;
                    _context?.Devices.Update(SelectedDevice);
                }
                    break;
            }
        };

        // load the entities into EF Core
        _context.Pools.Load();
        _context.Devices.Load();
        // 刷新设备列表
        Pools = _context.Pools.Local.ToObservableCollection();
        Devices = _context.Devices.Local.ToObservableCollection();
        SelectedDevice = Devices.FirstOrDefault();
    }

    public IRelayCommand DeleteDeviceCommand { get; }

    private void DoDeleteDevice()
    {
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

    private Device _selectedDevice;

    public Device SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }

    private Pool _selectedPool;

    public Pool SelectedPool
    {
        get => _selectedPool;
        set => SetProperty(ref _selectedPool, value);
    }

    public ObservableCollection<Pool> Pools { get; set; }
}