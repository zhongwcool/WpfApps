using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App19.Models;
using App19.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;

namespace App19.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel(ISnackbarMessageQueue snackbarMessageQueue)
    {
        NavigationItems = new ObservableCollection<NavigationItem>
        {
            new(
                "Column Chart",
                typeof(PageColumn),
                PackIconKind.TransitionMasked,
                PackIconKind.Transition)
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
            "Chart Sketch",
            typeof(PageSketch),
            PackIconKind.NaturePeople,
            PackIconKind.NaturePeopleOutline);

        yield return new NavigationItem(
            "Audio Analyzer",
            typeof(PageAudioAnalyzer),
            PackIconKind.Palette,
            PackIconKind.PaletteOutline);
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