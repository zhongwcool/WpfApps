using System.Collections.ObjectModel;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class DevicesTabViewModel : ObservableObject
{
    private IaContext Context { get; }

    public ObservableCollection<Device> NodesCollection { get; set; }

    private Device _selectedDevice = new();

    public Device SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }

    public DevicesTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.Devices.Load();
        // bind to the source
        NodesCollection = Context.Devices.Local.ToObservableCollection();
    }
}