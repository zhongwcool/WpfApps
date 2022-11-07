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

    private Coupon _coupon;

    public virtual Coupon Coupon
    {
        get => _coupon;
        set => SetProperty(ref _coupon, value);
    }
}