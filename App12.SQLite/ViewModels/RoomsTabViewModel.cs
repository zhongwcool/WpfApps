using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using App12.SQLite.HotelDbContext;
using App12.SQLite.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

namespace App12.SQLite.ViewModels;

public class RoomsTabViewModel : ObservableObject
{
    private Room _selectedRoom;
    private IList<Room> _filteredRoomList;
    public ObservableCollection<Room> RoomsCollection { get; set; }

    private HotelContext Context { get; }
    public Room RoomInfo { get; set; } = new();
    public Room RoomFilter { get; set; } = new();
    public int RoomFreedomFilterIndex { get; set; }

    public Room SelectedRoom
    {
        get => _selectedRoom;
        set => SetProperty(ref _selectedRoom, value);
    }

    public IList<Room> FilteredRoomList
    {
        get => _filteredRoomList;
        set => SetProperty(ref _filteredRoomList, value);
    }

    public RoomsTabViewModel(HotelContext context)
    {
        Context = context;
        // load the entities into EF Core
        Context.Rooms.Load();
        // bind to the source
        RoomsCollection = Context.Rooms.Local.ToObservableCollection();

        AddRoomCommand = new RelayCommand(AddRoom, CanExecuteAddRoom);
        UpdateRoomCommand = new RelayCommand(UpdateRoom, CanExecuteUpdateRoom);
        DeleteRoomCommand = new RelayCommand(DeleteRoom, CanExecuteDeleteRoom);
        ResetFilterRoomCommand = new RelayCommand<object>(ResetFilterRoom, CanExecuteResetFilterRoom);
        RoomsGridSelectionChangedCommand =
            new RelayCommand(RoomsGridSelectionChanged, CanExecuteRoomsGridSelectionChanged);
        RoomsFilterChangedCommand = new RelayCommand(RoomsFilterChanged);

        RoomInfo.PropertyChanged += RoomInfo_PropertyChanged;
        RoomFilter.PropertyChanged += RoomFilter_PropertyChanged;
    }

    private void RoomInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        AddRoomCommand.NotifyCanExecuteChanged();
        UpdateRoomCommand.NotifyCanExecuteChanged();
        DeleteRoomCommand.NotifyCanExecuteChanged();
    }

    private void RoomFilter_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        ResetFilterRoomCommand.NotifyCanExecuteChanged();
    }

    #region Commands

    public IRelayCommand AddRoomCommand { get; }

    private void AddRoom()
    {
        Context.Rooms.Add(new Room
        {
            Number = RoomInfo.Number,
            Type = RoomInfo.Type
        });
        Context.SaveChanges();
    }

    private bool CanExecuteAddRoom()
    {
        if (string.IsNullOrEmpty(RoomInfo.Number) || RoomInfo.Type == RoomTypes.None)
        {
            return false;
        }

        return true;
    }

    public IRelayCommand UpdateRoomCommand { get; }

    private void UpdateRoom()
    {
        SelectedRoom.Number = RoomInfo.Number;
        SelectedRoom.Type = RoomInfo.Type;
        Context.SaveChanges();
    }

    private bool CanExecuteUpdateRoom()
    {
        if (SelectedRoom == null) return false;
        if (string.IsNullOrEmpty(RoomInfo.Number) || RoomInfo.Type == RoomTypes.None)
        {
            return false;
        }

        return true;
    }

    public IRelayCommand DeleteRoomCommand { get; }

    private void DeleteRoom()
    {
        Context.Rooms.Remove(SelectedRoom);
        Context.SaveChanges();
    }

    private bool CanExecuteDeleteRoom()
    {
        return SelectedRoom != null;
    }

    public RelayCommand<object> ResetFilterRoomCommand { get; }

    private void ResetFilterRoom(object parameters)
    {
        if (parameters is Tuple<TextBox, ComboBox, ComboBox> tuple)
        {
            tuple.Item1.Text = string.Empty;
            tuple.Item2.SelectedIndex = 0;
            tuple.Item3.SelectedIndex = 0;
        }
    }

    private bool CanExecuteResetFilterRoom(object parameters)
    {
        if (parameters is Tuple<TextBox, ComboBox, ComboBox> tuple)
        {
            if (string.IsNullOrEmpty(tuple.Item1.Text)
                && (tuple.Item2 == null || new List<int> { -1, 0 }.IndexOf(tuple.Item2.SelectedIndex) != -1)
                && (tuple.Item3 == null || new List<int> { -1, 0 }.IndexOf(tuple.Item3.SelectedIndex) != -1))
                return false;
            return true;
        }

        return false;
    }

    public IRelayCommand RoomsGridSelectionChangedCommand { get; }

    private void RoomsGridSelectionChanged()
    {
        RoomInfo.Number = SelectedRoom.Number;
        RoomInfo.Type = SelectedRoom.Type;
    }

    private bool CanExecuteRoomsGridSelectionChanged()
    {
        return SelectedRoom != null;
    }

    public IRelayCommand RoomsFilterChangedCommand { get; }

    private void RoomsFilterChanged()
    {
        IEnumerable<Room> queryResult = Context.Rooms.Local;
        if (!string.IsNullOrEmpty(RoomFilter.Number))
        {
            queryResult = queryResult.Where(room => room.Number.Contains(RoomFilter.Number));
        }

        if (RoomFilter.Type != RoomTypes.None)
        {
            queryResult = queryResult.Where(room => room.Type == RoomFilter.Type);
        }

        if (RoomFreedomFilterIndex == 1)
        {
            queryResult = queryResult.Where(room => room.Clients.Count == 0);
        }

        if (RoomFreedomFilterIndex == 2)
        {
            queryResult = queryResult.Where(room => room.Clients.Count > 0);
        }

        FilteredRoomList = queryResult?.ToList();
    }

    #endregion
}