using System;
using System.ComponentModel;
using System.Windows;
using App12.SQLite.HotelDbContext;
using App12.SQLite.Models;
using App12.SQLite.ViewModels;

namespace App12.SQLite.Views;

public partial class MainWindow : Window
{
    private HotelContext Context { get; }

    public MainWindow()
    {
        InitializeComponent();

        RoomTypeCb.ItemsSource = RtCbFilter.ItemsSource = Enum.GetNames(typeof(RoomTypes));

        Context = new HotelContext();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // this is for demo purposes only, to make it easier
        // to get up and running
        Context.Database.EnsureCreated();
        // uncomment if you want to fill database with default values
        //Fill();

        // bind to the source
        ClientsTab.DataContext = new ClientsTabViewModel(Context);
        RoomsTab.DataContext = new RoomsTabViewModel(Context);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        // clean up database connections
        Context.Dispose();
        base.OnClosing(e);
    }

    private void Fill()
    {
        var rooms = new[]
        {
            new Room { Number = "1001", Type = RoomTypes.StandardRoom },
            new Room { Number = "1101", Type = RoomTypes.JuniorSuite },
            new Room { Number = "1003", Type = RoomTypes.StandardRoom },
            new Room { Number = "1201", Type = RoomTypes.PresidentialSuite },
            new Room { Number = "1102", Type = RoomTypes.JuniorSuite },
            new Room { Number = "1006", Type = RoomTypes.StandardRoom },
            new Room { Number = "1202", Type = RoomTypes.PresidentialSuite }
        };

        var coupons = new[]
        {
            new Coupon { CouponId = "CC20221107A", Discount = 9, CouponName = "双十一" },
            new Coupon { CouponId = "CC20221107B", Discount = 8, CouponName = "双十二" },
            new Coupon { CouponId = "CC20221107C", Discount = 6, CouponName = "新年大吉" },
        };

        var clients = new[]
        {
            new Client
            {
                FirstName = "Stanislav", LastName = "Herasymiuk", Birthdate = new DateTime(1995, 9, 2),
                Account = "stas_the_best", Room = rooms[1], Coupon = coupons[0]
            },
            new Client
            {
                FirstName = "Bob", LastName = "Marley", Birthdate = new DateTime(1952, 3, 25), Account = "919191",
                Room = rooms[3]
            },
            new Client
            {
                FirstName = "Frank", LastName = "Sinatra", Birthdate = new DateTime(1957, 7, 3), Account = "100500",
                Room = rooms[3], Coupon = coupons[2]
            },
            new Client
            {
                FirstName = "Phill", LastName = "Colson", Birthdate = new DateTime(1966, 12, 6),
                Account = "S.H.I.E.L.D.", Room = rooms[5], Coupon = coupons[1]
            },
            new Client
            {
                FirstName = "Dayzee", LastName = "Skay", Birthdate = new DateTime(1989, 10, 30), Account = "Hydra",
                Room = rooms[4]
            },
            new Client
            {
                FirstName = "Elvis", LastName = "Presley", Birthdate = new DateTime(1960, 2, 17),
                Account = "YA krevedko", Room = rooms[6]
            }
        };

        Context.Rooms.AddRange(rooms);
        Context.Clients.AddRange(clients);
        Context.Coupons.AddRange(coupons);
        Context.SaveChanges();
    }
}