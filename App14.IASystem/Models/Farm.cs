using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App14.IASystem.Models;

public class Farm : ObservableObject
{
    [Key] public Guid Id { get; set; }

    private string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private int _gongYang;

    public int GongYang
    {
        get => _gongYang;
        set => SetProperty(ref _gongYang, value);
    }

    private int _shiFei;

    public int ShiFei
    {
        get => _shiFei;
        set => SetProperty(ref _shiFei, value);
    }

    private int _touWei;

    public int TouWei
    {
        get => _touWei;
        set => SetProperty(ref _touWei, value);
    }

    private int _xiaoDu;

    public int XiaoDu
    {
        get => _xiaoDu;
        set => SetProperty(ref _xiaoDu, value);
    }

    private IList<Pool> _pools;

    public virtual IList<Pool> Pools
    {
        get => _pools;
        set => SetProperty(ref _pools, value);
    }
}