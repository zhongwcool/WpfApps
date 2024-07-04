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