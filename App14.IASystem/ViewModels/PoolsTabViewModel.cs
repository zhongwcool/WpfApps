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

public class PoolsTabViewModel : ObservableValidator, IDisposable
{
    private readonly IaContext _context;

    public PoolsTabViewModel()
    {
        RemovePoolCommand = new RelayCommand(DoRemoveSelectedPool, CanExecute_RemoveSelectedPoolCommand);
        RemoveNodeCommand = new RelayCommand(DoRemoveSelectedNode, CanExecute_RemoveSelectedNodeCommand);
        AddPoolCommand = new RelayCommand(DoAddPool);
        SaveCommand = new RelayCommand(DoSave);

        _context = new IaContext();
        // load the entities into EF Core
        _context.Farms.Load();
        _context.Devices.Load();
        _context.Pools.Load();
        // bind to the source
        Pools = new ObservableCollection<Pool>(_context.Pools.OrderBy(pool => pool.Name).ToList());
        SelectedPool = Pools.FirstOrDefault();

        OwnerList = _context.Pools.Select(pool => new Owner { Pool = pool, Description = pool.Name })
            .OrderBy(owner => owner.Description).ToList();
        OwnerList.Insert(0, new Owner { Pool = null, Description = "None" });

        PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(SelectedPool):
                    RemovePoolCommand.NotifyCanExecuteChanged();
                    break;
                case nameof(SelectedNode):
                    RemoveNodeCommand.NotifyCanExecuteChanged();
                    IsNodeSelected = null == SelectedNode ? IsNodeSelected = false : IsNodeSelected = true;
                    break;
            }
        };
    }

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
        set => SetProperty(ref _selectedNode, value);
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
            Farm = _context.Farms.FirstOrDefault()
        };
        _context.Pools.Add(SelectedPool);
        Pools.Add(SelectedPool);
    }

    public IRelayCommand RemovePoolCommand { get; }

    private void DoRemoveSelectedPool()
    {
        _context.Pools.Remove(SelectedPool);
        Pools.Remove(SelectedPool);
    }

    private bool CanExecute_RemoveSelectedPoolCommand()
    {
        return SelectedPool != null && 0 != SelectedPool.PoolId.CompareTo(Guid.Empty);
    }

    public IRelayCommand RemoveNodeCommand { get; }

    private void DoRemoveSelectedNode()
    {
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
            _context.SaveChanges();
        }
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

    ~PoolsTabViewModel()
    {
        Dispose(false);
    }
}