using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace App12.SQLite.Models;

public enum RoomTypes
{
    None,
    StandardRoom,
    BusinessClassRoom,
    JuniorSuite,
    PresidentialSuite
}

public class Room : ObservableObject
{
    public Room()
    {
        _clients = new List<Client>();
    }

    private int _roomId;

    public int RoomId
    {
        get => _roomId;
        set => SetProperty(ref _roomId, value);
    }

    private string _number;

    public string Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    private RoomTypes _type;

    public RoomTypes Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private IList<Client> _clients;

    public virtual IList<Client> Clients
    {
        get => _clients;
        set => SetProperty(ref _clients, value);
    }
}