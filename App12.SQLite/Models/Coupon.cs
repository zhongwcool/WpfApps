using CommunityToolkit.Mvvm.ComponentModel;

namespace App12.SQLite.Models;

public class Coupon : ObservableObject
{
    private string _couponId;

    public string CouponId
    {
        get => _couponId;
        set => SetProperty(ref _couponId, value);
    }

    private int _discount;

    public int Discount
    {
        get => _discount;
        set => SetProperty(ref _discount, value);
    }

    private string _couponName;

    public string CouponName
    {
        get => _couponName;
        set => SetProperty(ref _couponName, value);
    }
}