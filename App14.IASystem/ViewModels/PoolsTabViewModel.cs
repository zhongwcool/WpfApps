using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public Pool SelectedNode { get; set; }

    public PoolsTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.Pools.Load();
        // bind to the source
        PoolsCollection = Context.Pools.Local.ToObservableCollection();

        NodesGridSelectionChangedCommand =
            new RelayCommand(NodesGridSelectionChanged, CanExecuteNodesGridSelectionChanged);

        SelectedPool.PropertyChanged += PoolInfoOnPropertyChanged;
    }

    private void PoolInfoOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //TODO active selected item's operation
    }

    public IRelayCommand NodesGridSelectionChangedCommand { get; }

    private void NodesGridSelectionChanged()
    {
    }

    private bool CanExecuteNodesGridSelectionChanged()
    {
        return SelectedNode != null;
    }
}