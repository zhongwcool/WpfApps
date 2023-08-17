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

public class PoolsTabViewModel : ObservableValidator
{
    private IaContext Context { get; }
    public ObservableCollection<Pool> Pools { get; set; }

    private Pool _selectedPool = new();

    public Pool SelectedPool
    {
        get => _selectedPool;
        set => SetProperty(ref _selectedPool, value);
    }

    private Device _selectedNode;

    public Device SelectedNode
    {
        get => _selectedNode;
        set
        {
            SetProperty(ref _selectedNode, value);

            IsNodeSelected = (null != _selectedNode);

            if (null == _selectedNode?.Pool)
            {
                _selectedPoolIndex = 0;
                OnPropertyChanged(nameof(SelectedPoolIndex));
                return;
            }

            for (var j = 0; j < OwnerList.Count; j++)
            {
                if (null == OwnerList[j].Pool || OwnerList[j].Pool.PoolId != _selectedNode.Pool.PoolId) continue;
                _selectedPoolIndex = j;
                OnPropertyChanged(nameof(SelectedPoolIndex));
                break;
            }

            RemoveNodeCommand.NotifyCanExecuteChanged();
        }
    }

    public PoolsTabViewModel(IaContext iaContext)
    {
        RemovePoolCommand = new RelayCommand(DoRemoveSelectedPool, CanExecute_RemoveSelectedPoolCommand);
        RemoveNodeCommand = new RelayCommand(DoRemoveSelectedNode, CanExecute_RemoveSelectedNodeCommand);
        AddPoolCommand = new RelayCommand(DoAddPool);
        SaveCommand = new RelayCommand(DoSave);
        
        Context = iaContext;
        // load the entities into EF Core
        Context.Pools.Load();
        // bind to the source
        Pools = new ObservableCollection<Pool>(Context.Pools.OrderBy(pool => pool.Name).ToList());
        SelectedPool = Pools.FirstOrDefault();
        
        OwnerList = Context.Pools.Local.Select(pool => new Owner { Pool = pool, Description = pool.Name }).ToList();
        OwnerList.Insert(0, new Owner { Pool = null, Description = "None" });

        if (SelectedPool != null)
            SelectedPool.PropertyChanged += (sender, args) =>
            {
                RemovePoolCommand.NotifyCanExecuteChanged();
                RemoveNodeCommand.NotifyCanExecuteChanged();
            };
    }

    private bool _isNodeSelected = false;

    public bool IsNodeSelected
    {
        get => _isNodeSelected;
        set => SetProperty(ref _isNodeSelected, value);
    }

    public IRelayCommand AddPoolCommand { get; }

    private void DoAddPool()
    {
        SelectedPool = new Pool
        {
            Name = "New Pool",
            Farm = Context.Farms.FirstOrDefault()
        };
        Context.Pools.Add(SelectedPool);
        Pools.Add(SelectedPool);
    }

    public IRelayCommand RemovePoolCommand { get; }

    private void DoRemoveSelectedPool()
    {
        Context.Pools.Remove(SelectedPool);
        Pools.Remove(SelectedPool);
    }

    private bool CanExecute_RemoveSelectedPoolCommand()
    {
        return SelectedPool != null && 0 != SelectedPool.PoolId.CompareTo(Guid.Empty);
    }

    public IRelayCommand RemoveNodeCommand { get; }

    private void DoRemoveSelectedNode()
    {
        //SelectedNode.Pool = null;
        SelectedPool.Devices.Remove(SelectedNode);
    }

    private bool CanExecute_RemoveSelectedNodeCommand()
    {
        return SelectedPool != null && SelectedNode != null;
    }

    public IRelayCommand SaveCommand { get; }

    private void DoSave()
    {
        ValidateAllProperties();
        if (!HasErrors)
        {
            Context.SaveChanges();
        }
    }

    private int _selectedPoolIndex = 0;

    public int SelectedPoolIndex
    {
        get => _selectedPoolIndex;
        set
        {
            SetProperty(ref _selectedPoolIndex, value);
            SelectedNode.Pool = OwnerList[_selectedPoolIndex].Pool;
            Context.SaveChanges();
        }
    }

    public List<Owner> OwnerList { get; }
}