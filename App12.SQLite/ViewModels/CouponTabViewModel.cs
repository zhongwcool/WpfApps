using System.Collections.ObjectModel;
using App12.SQLite.HotelDbContext;
using App12.SQLite.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App12.SQLite.ViewModels;

public class CouponTabViewModel : ObservableObject
{
    private readonly HotelContext _context;

    public CouponTabViewModel(HotelContext context)
    {
        _context = context;

        Coupons = _context.Coupons.Local.ToObservableCollection();
    }

    public ObservableCollection<Coupon> Coupons { get; set; }
}