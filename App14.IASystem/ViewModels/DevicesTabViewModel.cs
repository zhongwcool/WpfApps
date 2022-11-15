using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        set
        {
            SetProperty(ref _selectedDevice, value);

            if (null == _selectedDevice.Pool)
            {
                SelectedIndex = 0;
                return;
            }

            for (var j = 0; j < OwnerList.Count; j++)
            {
                if (null == OwnerList[j].Pool || OwnerList[j].Pool.PoolId != _selectedDevice.Pool.PoolId) continue;
                SelectedIndex = j;
                break;
            }
        }
    }

    private int _selectedIndex = 0;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            SetProperty(ref _selectedIndex, value);
            SelectedDevice.Pool = OwnerList[_selectedIndex].Pool;
            Context.SaveChanges();
        }
    }

    public List<Owner> OwnerList { get; }

    public DevicesTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.Devices.Load();
        // bind to the source
        NodesCollection = Context.Devices.Local.ToObservableCollection();

        OwnerList = Context.Pools.Local.Select(pool => new Owner { Pool = pool, Description = pool.Name }).ToList();
        OwnerList.Insert(0, new Owner { Pool = null, Description = "None" });
    }
}