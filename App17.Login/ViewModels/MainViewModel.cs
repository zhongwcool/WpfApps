using App17.Login.Data;
using App17.Login.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App17.Login.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        var client = new ZyClient();
        client.DataReceived += OnDataReceived;
    }

    private void OnDataReceived(ZyWater water)
    {
        Water = water;
        TxtRepo = water.Html;
    }

    private ZyWater _water;

    public ZyWater Water
    {
        get => _water;
        set => SetProperty(ref _water, value);
    }

    private string _txtRepo = "Hello World";

    public string TxtRepo
    {
        get => _txtRepo;
        set => SetProperty(ref _txtRepo, value);
    }
}