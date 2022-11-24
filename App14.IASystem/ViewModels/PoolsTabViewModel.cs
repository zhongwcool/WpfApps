using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class PoolsTabViewModel : ObservableObject
{
    private IaContext Context { get; }
    public ObservableCollection<Pool> PoolsCollection { get; set; }

    private Pool _selectedPool = new();

    public Pool SelectedPool
    {
        get => _selectedPool;
        set => SetProperty(ref _selectedPool, value);
    }

    private Device _selectedDevice;

    public Device SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            SetProperty(ref _selectedDevice, value);

            if (null == _selectedDevice?.Pool)
            {
                _selectedIndex = 0;
                OnPropertyChanged(nameof(SelectedIndex));
                return;
            }

            for (var j = 0; j < OwnerList.Count; j++)
            {
                if (null == OwnerList[j].Pool || OwnerList[j].Pool.PoolId != _selectedDevice.Pool.PoolId) continue;
                _selectedIndex = j;
                OnPropertyChanged(nameof(SelectedIndex));
                break;
            }
        }
    }

    public Device DeviceInfo { get; set; } = new();

    public PoolsTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.Pools.Load();
        // bind to the source
        PoolsCollection = Context.Pools.Local.ToObservableCollection();
        OwnerList = Context.Pools.Local.Select(pool => new Owner { Pool = pool, Description = pool.Name }).ToList();
        OwnerList.Insert(0, new Owner { Pool = null, Description = "None" });

        RemoveCommand = new RelayCommand(DoRemove, CanExecute_RemoveCommand);

        DevicesGridSelectionChangedCommand =
            new RelayCommand(DevicesGridSelectionChanged, CanExecuteDevicesGridSelectionChanged);
        DeviceInfo.PropertyChanged += NodeInfo_PropertyChanged;
    }

    private void NodeInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        RemoveCommand.NotifyCanExecuteChanged();
    }

    public IRelayCommand DevicesGridSelectionChangedCommand { get; }

    private void DevicesGridSelectionChanged()
    {
        DeviceInfo.NodeName = SelectedDevice.NodeName;
    }

    private bool CanExecuteDevicesGridSelectionChanged()
    {
        return SelectedDevice != null;
    }

    public IRelayCommand RemoveCommand { get; }

    private void DoRemove()
    {
        SelectedDevice.Pool = null;
        Context.SaveChanges();
    }

    private bool CanExecute_RemoveCommand()
    {
        return SelectedDevice != null;
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
}