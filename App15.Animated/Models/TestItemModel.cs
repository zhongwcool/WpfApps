using CommunityToolkit.Mvvm.ComponentModel;

namespace App15.Animated.Models;

public class TestItemModel : ObservableObject
{
    private string _productName;

    public string ProductName
    {
        get => _productName;
        set => SetProperty(ref _productName, value);
    }

    private string _productDescription;

    public string ProductDescription
    {
        get => _productDescription;
        set => SetProperty(ref _productDescription, value);
    }

    private double _productPrice;

    public double ProductPrice
    {
        get => _productPrice;
        set => SetProperty(ref _productPrice, value);
    }
}