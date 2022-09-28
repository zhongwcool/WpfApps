using System.Collections.ObjectModel;
using App15.Animated.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App15.Animated.ViewModels;

public class MainViewModel : ObservableObject
{
    public ObservableCollection<TestItemModel> TestItems { get; set; }

    public MainViewModel()
    {
        TestItems = new ObservableCollection<TestItemModel>();

        TestItems.Add(new TestItemModel
            { ProductName = "Xbox One", ProductDescription = "This product is ok.", ProductPrice = 200.99 });
        TestItems.Add(new TestItemModel
            { ProductName = "Playstation 4", ProductDescription = "a", ProductPrice = 250.99 });
        TestItems.Add(new TestItemModel
            { ProductName = "PC", ProductDescription = "AMD, AMD, AMD, AMD, AMD and Samsung", ProductPrice = 749.99 });
        TestItems.Add(new TestItemModel
            { ProductName = "Yacht", ProductDescription = "Fast", ProductPrice = 450000.99 });
        TestItems.Add(new TestItemModel
        {
            ProductName = "Bob the builder's antimatter generator",
            ProductDescription = "Only uses 193.6 terajoules per second.", ProductPrice = 999999.99
        });
        TestItems.Add(new TestItemModel
            { ProductName = "Playstation 4", ProductDescription = "repeating because h", ProductPrice = 250.99 });
    }
}