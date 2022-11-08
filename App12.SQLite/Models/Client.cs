using System.Collections.Generic;

namespace App12.SQLite.Models;

public class Client : Person
{
    private string _account;

    public string Account
    {
        get => _account;
        set => SetProperty(ref _account, value);
    }

    private Room _room;

    public virtual Room Room
    {
        get => _room;
        set => SetProperty(ref _room, value);
    }

    private IList<Coupon> _coupons;

    public virtual IList<Coupon> Coupons
    {
        get => _coupons;
        set => SetProperty(ref _coupons, value);
    }
}