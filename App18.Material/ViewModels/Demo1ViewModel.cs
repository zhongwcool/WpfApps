using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App18.Material.Models;
using App18.Material.Views.Demo;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace App18.Material.ViewModels;

public class Demo1ViewModel : ObservableObject
{
    public Demo1ViewModel()
    {
        Demo1Items = new ObservableCollection<Demo1Item>
        {
            new(
                nameof(PageChild1),
                typeof(PageChild1),
                PackIconKind.AlphaABox,
                PackIconKind.AlphaABoxOutline)
        };

        foreach (var item in GenerateItems().OrderBy(i => i.Name))
            Demo1Items.Add(item);

        SelectedItem = Demo1Items.First();
    }

    private static IEnumerable<Demo1Item> GenerateItems()
    {
        yield return new Demo1Item(
            nameof(PageChild2),
            typeof(PageChild2),
            PackIconKind.AlphaBBox,
            PackIconKind.AlphaBBoxOutline);

        yield return new Demo1Item(
            nameof(PageChild3),
            typeof(PageChild3),
            PackIconKind.AlphaCBox,
            PackIconKind.AlphaCBoxOutline);
    }

    public ObservableCollection<Demo1Item> Demo1Items { get; }

    private Demo1Item? _selectedItem;

    public Demo1Item? SelectedItem
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
}