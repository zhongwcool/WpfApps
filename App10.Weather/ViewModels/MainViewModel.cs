using System.IO;
using System.Linq;
using System.Net.Http;
using App10.Weather.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App10.Weather.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;

    private MainViewModel()
    {
        CommandRenew = new RelayCommand(() => { RequestWeatherData("29.5617:120.0962"); });

        LoadDataFromJson();
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    public IRelayCommand CommandRenew { get; }

    private readonly HttpClient _httpClient = new();

    private async void RequestWeatherData(string location)
    {
        var url =
            $"https://api.seniverse.com/v3/weather/now.json?key=SonT2HaJQRjGe-C_E&location={location}&language=zh-Hans&unit=c";
        var data = await _httpClient.GetStringAsync(url);
        ParseDataFromJson(data);
        SaveDataToJson(data);
    }

    private WeatherModel _weatherModel;

    public WeatherModel WeatherModel
    {
        get => _weatherModel;
        set => SetProperty(ref _weatherModel, value);
    }

    /// <summary>
    /// parse the data we want form the json
    /// </summary>
    /// <param name="json"></param>
    /// <returns>weather object</returns>
    private void ParseDataFromJson(string json)
    {
        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        if (result1 == null) return;
        WeatherModel = JsonConvert.DeserializeObject<WeatherModel>(result1.ToString());
    }

    /// <summary>
    /// save data to json file
    /// </summary>
    /// <param name="data"></param>
    private static void SaveDataToJson(string data)
    {
        using Stream s = File.Open("data.Json", FileMode.Create);
        using var sw = new StreamWriter(s);
        sw.WriteLine(data);
    }

    /// <summary>
    /// Load data from json
    /// </summary>
    /// <returns>json string</returns>
    private void LoadDataFromJson()
    {
        using Stream s = File.OpenRead("data.Json");
        using var sr = new StreamReader(s);
        var json = sr.ReadToEnd();

        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        if (result1 == null) return;
        WeatherModel = JsonConvert.DeserializeObject<WeatherModel>(result1.ToString());
    }
}