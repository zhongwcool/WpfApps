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

public class PoolsTabViewModel : ObservableObject
{
    private IaContext Context { get; }
    public ObservableCollection<Pool> PoolsCollection { get; set; }

    private Pool _selectedPool = new();

    public Pool SelectedPool
    {
        get => _selectedPool;
        set
        {
            SetProperty(ref _selectedPool, value);
            RemoveCommand.NotifyCanExecuteChanged();
        }
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
                _selectedPoolIndex = 0;
                OnPropertyChanged(nameof(SelectedPoolIndex));
                return;
            }

            for (var j = 0; j < OwnerList.Count; j++)
            {
                if (null == OwnerList[j].Pool || OwnerList[j].Pool.PoolId != _selectedDevice.Pool.PoolId) continue;
                _selectedPoolIndex = j;
                OnPropertyChanged(nameof(SelectedPoolIndex));
                break;
            }
        }
    }

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
        SaveCommand = new RelayCommand(DoSave);
    }

    public IRelayCommand RemoveCommand { get; }

    private void DoRemove()
    {
        Context.Remove(SelectedPool);
        Context.SaveChanges();
    }

    public IRelayCommand SaveCommand { get; }

    private void DoSave()
    {
        Context.SaveChanges();
    }

    private bool CanExecute_RemoveCommand()
    {
        return SelectedPool != null && 0 != SelectedPool.PoolId.CompareTo(Guid.Empty);
    }

    private int _selectedPoolIndex = 0;

    public int SelectedPoolIndex
    {
        get => _selectedPoolIndex;
        set
        {
            SetProperty(ref _selectedPoolIndex, value);
            SelectedDevice.Pool = OwnerList[_selectedPoolIndex].Pool;
            Context.SaveChanges();
        }
    }

    public List<Owner> OwnerList { get; }
}