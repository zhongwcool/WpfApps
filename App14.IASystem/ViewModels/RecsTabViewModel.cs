using System.Collections.ObjectModel;
using App14.IASystem.Context;
using App14.IASystem.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace App14.IASystem.ViewModels;

public class RecsTabViewModel : ObservableObject
{
    private IaContext Context { get; }

    public ObservableCollection<RecWqm> WqmsCollection { get; set; }

    public RecsTabViewModel(IaContext iaContext)
    {
        Context = iaContext;
        // load the entities into EF Core
        Context.RecWqms.Load();
        // bind to the source
        WqmsCollection = Context.RecWqms.Local.ToObservableCollection();
    }

    private Device _selectedRecWqm = new();

    public Device SelectedRecWqm
    {
        get => _selectedRecWqm;
        set => SetProperty(ref _selectedRecWqm, value);
    }
}