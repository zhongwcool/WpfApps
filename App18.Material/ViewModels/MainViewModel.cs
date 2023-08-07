using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App18.Material.Models;
using App18.Material.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;

namespace App18.Material.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel(ISnackbarMessageQueue snackbarMessageQueue)
    {
        NavigationItems = new ObservableCollection<NavigationItem>
        {
            new(
                nameof(PageHome),
                typeof(PageHome),
                PackIconKind.Palette,
                PackIconKind.PaletteOutline)
        };

        foreach (var item in GenerateNavigationItems(snackbarMessageQueue).OrderBy(i => i.Name))
            NavigationItems.Add(item);

        SelectedItem = NavigationItems.First();

        AddNewNotiCommand = new RelayCommand(AddNewNotification);
        DismissAllCommand = new RelayCommand(DismissAllNotifications);
    }

    private static IEnumerable<NavigationItem> GenerateNavigationItems(ISnackbarMessageQueue snackbarMessageQueue)
    {
        if (snackbarMessageQueue is null)
            throw new ArgumentNullException(nameof(snackbarMessageQueue));

        yield return new NavigationItem(
            nameof(PageDemo1),
            typeof(PageDemo1),
            PackIconKind.Alien,
            PackIconKind.AlienOutline);

        yield return new NavigationItem(
            nameof(PageDemo2),
            typeof(PageDemo2),
            PackIconKind.Butterfly,
            PackIconKind.ButterflyOutline);
    }

    public ObservableCollection<NavigationItem> NavigationItems { get; }

    private NavigationItem? _selectedItem;

    public NavigationItem? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    private int _selectedIndex;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    private int _notificationNumber;

    public object? Notifications
    {
        get
        {
            if (_notificationNumber == 0) return null;
            return _notificationNumber < 100 ? _notificationNumber : "99+";
        }
    }

    public IRelayCommand DismissAllCommand { get; }
    public IRelayCommand AddNewNotiCommand { get; }

    private void AddNewNotification()
    {
        _notificationNumber++;
        OnPropertyChanged(nameof(Notifications));
    }

    private void DismissAllNotifications()
    {
        _notificationNumber = 0;
        OnPropertyChanged(nameof(Notifications));
    }
}