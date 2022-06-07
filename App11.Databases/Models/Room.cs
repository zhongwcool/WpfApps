﻿using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace App11.Databases.Models;

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
    private RoomTypes _type;
    private string _number;
    private int _roomId;
    private IList<Client> _clients;

    public int RoomId
    {
        get => _roomId;
        set => SetProperty(ref _roomId, value);
    }

    public string Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public RoomTypes Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    public virtual IList<Client> Clients
    {
        get => _clients;
        set => SetProperty(ref _clients, value);
    }

    public Room()
    {
        _clients = new List<Client>();
    }
}