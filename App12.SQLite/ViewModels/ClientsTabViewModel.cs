﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using App12.SQLite.HotelDbContext;
using App12.SQLite.Miscellaneous;
using App12.SQLite.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace App12.SQLite.ViewModels;

public class ClientsTabViewModel : ObservableObject
{
    private IList<Client> _filteredClientList;
    public ObservableCollection<Client> ClientsCollection { get; set; }
    public ObservableCollection<Room> RoomsCollection { get; set; }

    private HotelContext Context { get; }
    public Client ClientInfo { get; set; } = new Client();
    public Client ClientFilter { get; set; } = new Client();
    public Client SelectedClient { get; set; }

    public IList<Client> FilteredClientList
    {
        get => _filteredClientList;
        set => SetProperty(ref _filteredClientList, value);
    }

    public ClientsTabViewModel(HotelContext context)
    {
        Context = context;
        // load the entities into EF Core
        Context.Clients.Load();
        // bind to the source
        ClientsCollection = Context.Clients.Local.ToObservableCollection();
        RoomsCollection = Context.Rooms.Local.ToObservableCollection();

        AddClientCommand = new RelayCommand(AddClient, CanExecuteAddClient);
        UpdateClientCommand = new RelayCommand(UpdateClient, CanExecuteUpdateClient);
        DeleteClientCommand = new RelayCommand(DeleteClient, CanExecuteDeleteClient);
        ExportClientsCommand = new RelayCommand(ExportClients, CanExecuteExportClients);
        ResetFilterClientCommand = new RelayCommand<object>(ResetFilterClient, CanExecuteResetFilterClient);
        ClientsGridSelectionChangedCommand =
            new RelayCommand(ClientsGridSelectionChanged, CanExecuteClientsGridSelectionChanged);
        ClientsFilterChangedCommand = new RelayCommand(ClientsFilterChanged);

        ClientInfo.PropertyChanged += ClientInfo_PropertyChanged;
        ClientFilter.PropertyChanged += ClientFilter_PropertyChanged;
    }

    private void ClientInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        AddClientCommand.NotifyCanExecuteChanged();
        UpdateClientCommand.NotifyCanExecuteChanged();
        DeleteClientCommand.NotifyCanExecuteChanged();
    }

    private void ClientFilter_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        ResetFilterClientCommand.NotifyCanExecuteChanged();
    }

    #region Commands

    public RelayCommand AddClientCommand { get; }

    private void AddClient()
    {
        Context.Clients.Add(new Client
        {
            FirstName = ClientInfo.FirstName,
            LastName = ClientInfo.LastName,
            Birthdate = ClientInfo.Birthdate,
            Account = ClientInfo.Account,
            Room = ClientInfo.Room
        });
        Context.SaveChanges();
    }

    private bool CanExecuteAddClient()
    {
        if (string.IsNullOrEmpty(ClientInfo.FirstName)
            || string.IsNullOrEmpty(ClientInfo.LastName)
            || ClientInfo.Room == null)
        {
            return false;
        }

        return true;
    }

    public RelayCommand UpdateClientCommand { get; }

    private void UpdateClient()
    {
        SelectedClient.FirstName = ClientInfo.FirstName;
        SelectedClient.LastName = ClientInfo.LastName;
        SelectedClient.Birthdate = ClientInfo.Birthdate;
        SelectedClient.Account = ClientInfo.Account;
        SelectedClient.Room = ClientInfo.Room;
        Context.SaveChanges();
    }

    private bool CanExecuteUpdateClient()
    {
        if (SelectedClient == null) return false;
        if (string.IsNullOrEmpty(ClientInfo.FirstName)
            || string.IsNullOrEmpty(ClientInfo.LastName)
            || ClientInfo.Room == null)
        {
            return false;
        }

        return true;
    }

    public RelayCommand DeleteClientCommand { get; }

    private void DeleteClient()
    {
        Context.Clients.Remove(SelectedClient);
        Context.SaveChanges();
    }

    private bool CanExecuteDeleteClient()
    {
        return SelectedClient != null;
    }

    public RelayCommand ExportClientsCommand { get; }

    private void ExportClients()
    {
        var clientsExport = Context.Clients.Select(client => new ClientInfo
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            Birthdate = client.Birthdate.Value,
            Account = client.Account,
            RoomNumber = client.Room.Number
        });
        var saveDialog = new SaveFileDialog
        {
            DefaultExt = ".xls",
            Filter = "Excel table (.xls)|*.xls"
        };
        var result = saveDialog.ShowDialog();
        if (result != true) return;
        using TextWriter sw = new StreamWriter(saveDialog.FileName);
        var reportCreator = new ReportCreator();
        reportCreator.WriteTsv(clientsExport, sw);
        sw.Close();
    }

    private bool CanExecuteExportClients()
    {
        return Context.Clients.Count() != 0;
    }

    public RelayCommand<object> ResetFilterClientCommand { get; }

    private void ResetFilterClient(object parameters)
    {
        if (parameters is Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox> tuple)
        {
            tuple.Item1.Text = string.Empty;
            tuple.Item2.Text = string.Empty;
            tuple.Item3.SelectedDate = null;
            tuple.Item4.Text = string.Empty;
            tuple.Item5.SelectedIndex = -1;
        }
    }

    private bool CanExecuteResetFilterClient(object parameters)
    {
        if (parameters == null) return false;
        if (parameters is Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox> tuple)
        {
            if (string.IsNullOrEmpty(tuple.Item1.Text)
                && string.IsNullOrEmpty(tuple.Item2.Text)
                && tuple.Item3.SelectedDate == null
                && string.IsNullOrEmpty(tuple.Item4.Text)
                && (tuple.Item5 == null || tuple.Item5.SelectedIndex == -1))
                return false;
            return true;
        }

        return false;
    }

    public RelayCommand ClientsGridSelectionChangedCommand { get; }

    private void ClientsGridSelectionChanged()
    {
        ClientInfo.FirstName = SelectedClient.FirstName;
        ClientInfo.LastName = SelectedClient.LastName;
        ClientInfo.Birthdate = SelectedClient.Birthdate.Value;
        ClientInfo.Account = SelectedClient.Account;
        ClientInfo.Room = SelectedClient.Room;
    }

    private bool CanExecuteClientsGridSelectionChanged()
    {
        return SelectedClient != null;
    }

    public RelayCommand ClientsFilterChangedCommand { get; }

    private void ClientsFilterChanged()
    {
        IEnumerable<Client> queryResult = Context.Clients.Local;
        if (!string.IsNullOrEmpty(ClientFilter.FirstName))
        {
            queryResult = queryResult.Where(client => client.FirstName.Contains(ClientFilter.FirstName));
        }

        if (!string.IsNullOrEmpty(ClientFilter.LastName))
        {
            queryResult = queryResult.Where(client => client.LastName.Contains(ClientFilter.LastName));
        }

        if (ClientFilter.Birthdate != null)
        {
            queryResult = queryResult.Where(client => client.Birthdate == ClientFilter.Birthdate);
        }

        if (!string.IsNullOrEmpty(ClientFilter.Account))
        {
            queryResult = queryResult.Where(client => client.Account.Contains(ClientFilter.Account));
        }

        if (ClientFilter.Room != null)
        {
            queryResult = queryResult.Where(client => client.Room == ClientFilter.Room);
        }

        FilteredClientList = queryResult?.ToList();
    }

    #endregion
}

internal class ClientInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public string Account { get; set; }
    public string RoomNumber { get; set; }
}