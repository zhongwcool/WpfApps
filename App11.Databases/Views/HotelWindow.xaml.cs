using System;
using System.Data.Entity;
using System.Windows;
using App11.Databases.HotelDbContext;
using App11.Databases.Models;
using App11.Databases.ViewModels;

namespace App11.Databases.Views;

public partial class HotelWindow : Window
{
    private static HotelWindow _instance;

    public static HotelWindow GetInstance()
    {
        _instance ??= new HotelWindow();
        return _instance;
    }

    private HotelContext Context { get; }

    public HotelWindow()
    {
        InitializeComponent();

        RoomTypeCb.ItemsSource = RtCbFilter.ItemsSource = Enum.GetNames(typeof(RoomTypes));

        Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HotelContext>());
        //Database.SetInitializer(new DropCreateDatabaseAlways<HotelContext>()); // set it if you want to recreate database
        Context = new HotelContext();
        //Fill(); // uncomment if you want to fill database with default values
        ClientsTab.DataContext = new ClientsTabViewModel(Context);
        RoomsTab.DataContext = new RoomsTabViewModel(Context);
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

        var clients = new[]
        {
            new Client
            {
                FirstName = "Stanislav", LastName = "Herasymiuk", Birthdate = new DateTime(1995, 9, 2),
                Account = "stas_the_best", Room = rooms[1]
            },
            new Client
            {
                FirstName = "Bob", LastName = "Marley", Birthdate = new DateTime(1952, 3, 25), Account = "919191",
                Room = rooms[3]
            },
            new Client
            {
                FirstName = "Frank", LastName = "Sinatra", Birthdate = new DateTime(1957, 7, 3), Account = "100500",
                Room = rooms[3]
            },
            new Client
            {
                FirstName = "Phill", LastName = "Colson", Birthdate = new DateTime(1966, 12, 6),
                Account = "S.H.I.E.L.D.", Room = rooms[5]
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
        Context.SaveChanges();
    }
}