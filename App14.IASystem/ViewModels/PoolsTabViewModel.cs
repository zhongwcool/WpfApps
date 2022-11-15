using System;
using System.Collections.ObjectModel;
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

    public Pool PoolInfo { get; set; } = new Pool();

    private Device _selectedNote = new();

    public Device SelectedNode
    {
        get => _selectedNote;
        set => SetProperty(ref _selectedNote, value);
    }

    public Device NodeInfo { get; set; } = new Device();

    public PoolsTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.Pools.Load();
        // bind to the source
        PoolsCollection = Context.Pools.Local.ToObservableCollection();

        RemoveCommand = new RelayCommand(DoRemove, CanExecute_RemoveCommand);
        ChangeOwnerCommand = new RelayCommand(ChangeOwner, CanExecute_ChangeOwnerCommand);
    }

    public IRelayCommand RemoveCommand { get; }

    private void DoRemove()
    {
        SelectedNode.Pool = null;
        Context.SaveChanges();
    }

    private bool CanExecute_RemoveCommand()
    {
        if (SelectedNode.Id == Guid.Empty) return false;
        return true;
    }

    public IRelayCommand ChangeOwnerCommand { get; }

    private bool CanExecute_ChangeOwnerCommand()
    {
        return true;
    }

    private void ChangeOwner()
    {
        SelectedNode = null;
    }
}