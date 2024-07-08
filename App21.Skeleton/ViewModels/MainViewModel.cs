using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using App21.Skeleton.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Mar.Cheese;

namespace App21.Skeleton.ViewModels;

public class MainViewModel : ObservableObject
{
    public async Task LoadDataAsync()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var assembly = Assembly.GetEntryAssembly()?.GetName();
        var myAppFolder = Path.Combine(appDataPath, "App01.VLC");
        var jsonFile = Path.Combine(myAppFolder, JSON_FILE);

        await Task.Delay(TimeSpan.FromSeconds(2));

        try
        {
            var model = JsonUtil.Load<MyApp>(jsonFile);
            if (model == null) return;
            Channels.Clear();
            foreach (var channel in model.Channels)
            {
                Channels.Add(channel);
                if (!IsButtonChecked) Application.Current.Dispatcher.Invoke(() => IsButtonChecked = true);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private const string JSON_FILE = "app.json";

    public ObservableCollection<Channel> Channels { get; set; } = [];

    public ObservableCollection<Product> Products { get; set; } =
    [
        new Product
        {
            Title = "Anime Month: savings up to 80%", Note = "Celebrate and save on anime all month long",
            Tips = ["9.99$"],
            Source = "https://gd-hbimg.huaban.com/3bb312d3dd4754036f6e0ad0b5ebce6246c7c4ca200a3-9zhcor"
        },
        new Product
        {
            Title = "Product 2", Note = "This is the second product",
            Tips = ["Tip 4", "Tip 5", "Tip 6"],
            Source = "https://gd-hbimg.huaban.com/0b3b8c2562d89917e744a04065b4b21c033df17b19b743-dyiM8G_fw658webp"
        },
        new Product
        {
            Title = "Product 3", Note = "This is the third product",
            Tips = ["Free"],
            Source = "https://gd-hbimg.huaban.com/e046907e0e2f6dae57a9c08ae934a0bcd3ca552c1a1d17-b5BcWW_fw658webp"
        }
    ];

    private bool _isButtonChecked;

    public bool IsButtonChecked
    {
        get => _isButtonChecked;
        set
        {
            SetProperty(ref _isButtonChecked, value);
            DataIsLoaded = IsButtonChecked;
        }
    }

    private bool _dataIsLoaded;

    public bool DataIsLoaded
    {
        get => _dataIsLoaded;
        set => SetProperty(ref _dataIsLoaded, value);
    }
}