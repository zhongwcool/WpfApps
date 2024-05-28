using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
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
        ColorPresets =
        [
            new ColorPreset
            {
                Name = "清新自然风",
                Primary = (Color)ColorConverter.ConvertFromString("#4CAF50")!,
                Secondary = (Color)ColorConverter.ConvertFromString("#C8E6C9")!
            },
            new ColorPreset
            {
                Name = "沉稳高雅风",
                Primary = (Color)ColorConverter.ConvertFromString("#607D8B")!,
                Secondary = (Color)ColorConverter.ConvertFromString("#CFD8DC")!
            },
            new ColorPreset
            {
                Name = "活力四射风",
                Primary = (Color)ColorConverter.ConvertFromString("#E53935")!,
                Secondary = (Color)ColorConverter.ConvertFromString("#FFE0B2")!
            },
            new ColorPreset
            {
                Name = "海洋蓝调风",
                Primary = (Color)ColorConverter.ConvertFromString("#0288D1")!,
                Secondary = (Color)ColorConverter.ConvertFromString("#B3E5FC")!
            },
        ];
        SelectedColorPreset = ColorPresets.FirstOrDefault();

        NavigationItems =
        [
            new NavigationItem(
                nameof(PageHome),
                typeof(PageHome),
                PackIconKind.TransitionMasked,
                PackIconKind.Transition)
        ];

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
            PackIconKind.NaturePeople,
            PackIconKind.NaturePeopleOutline);

        yield return new NavigationItem(
            nameof(PageDemo2),
            typeof(PageDemo2),
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

    public object Notifications
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

    private ColorPreset _selectedColorPreset;

    public ColorPreset SelectedColorPreset
    {
        get => _selectedColorPreset;
        set
        {
            SetProperty(ref _selectedColorPreset, value);
            ChangePreset(_selectedColorPreset);
        }
    }

    private static void ChangePreset(ColorPreset preset)
    {
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();

        theme.SetPrimaryColor(preset.Primary);
        theme.SetSecondaryColor(preset.Secondary);

        var colorAdjustment = theme.ColorAdjustment ?? new ColorAdjustment();
        theme.ColorAdjustment = colorAdjustment;

        paletteHelper.SetTheme(theme);
    }

    public ObservableCollection<ColorPreset> ColorPresets { set; get; }
}