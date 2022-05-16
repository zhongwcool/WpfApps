using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using App10.Weather.Data;
using App10.Weather.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App10.Weather.ViewModels;

public class MainViewModel : ObservableObject
{
    private static MainViewModel _instance;
    private readonly HttpClient _httpClient = new();
    private readonly AppConfig _config = AppConfig.CreateInstance();

    private MainViewModel()
    {
        var location = _config.GetValue(Section.Other, "Location");
        var apikey = _config.GetValue(Section.Other, "SenKey");

        CommandRenewNow = new RelayCommand(() => RequestNow(apikey, location));
        CommandRenewHourly = new RelayCommand(() => { RequestHourly(apikey, location); });
        CommandRenewDaily = new RelayCommand(() => { RequestDaily(apikey, location); });

        LoadSavedData();
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    #region 逐日天气

    public IRelayCommand CommandRenewDaily { get; }

    private async void RequestDaily(string apikey, string location)
    {
        if (string.IsNullOrEmpty(location)) return;
        if (string.IsNullOrEmpty(apikey)) return;
        var url =
            $"https://api.seniverse.com/v3/weather/daily.json?key={apikey}&location={location}&language=zh-Hans&unit=c&start=0&days=5";
        var data = await _httpClient.GetStringAsync(url);
        ParseDataDaily(data);
        SaveDataDaily(data);
    }

    private DailyModel _dailyModel;

    public DailyModel DailyModel
    {
        get => _dailyModel;
        set => SetProperty(ref _dailyModel, value);
    }

    /// <summary>
    /// parse the data we want form the json
    /// </summary>
    /// <param name="json"></param>
    /// <returns>weather object</returns>
    private void ParseDataDaily(string json)
    {
        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        if (result1 == null) return;
        DailyModel = JsonConvert.DeserializeObject<DailyModel>(result1.ToString());

        WeakReferenceMessenger.Default.Send(new Message { Key = 102, Obj = DailyModel });
    }

    private const string FileDaily = "data_daily.json";

    private static void SaveDataDaily(string data)
    {
        SaveDataToJson(FileDaily, data);
    }

    private void LoadDataDaily()
    {
        var data = LoadDataFromJson(FileDaily);
        if (string.IsNullOrEmpty(data)) return;
        DailyModel = JsonConvert.DeserializeObject<DailyModel>(data);
    }

    #endregion

    #region 逐时天气 Hourly

    public IRelayCommand CommandRenewHourly { get; }

    private async void RequestHourly(string apikey, string location)
    {
        var url =
            $"https://api.seniverse.com/v3/weather/hourly.json?key={apikey}&location={location}&language=zh-Hans&unit=c&start=0&hours=24";
        var data = await _httpClient.GetStringAsync(url);
        ParseDataHourly(data);
        SaveDataHourly(data);
    }

    private HourlyModel _hourlyModel;

    public HourlyModel HourlyModel
    {
        get => _hourlyModel;
        set => SetProperty(ref _hourlyModel, value);
    }

    /// <summary>
    /// parse the data we want form the json
    /// </summary>
    /// <param name="json"></param>
    /// <returns>weather object</returns>
    private void ParseDataHourly(string json)
    {
        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        if (result1 == null) return;
        HourlyModel = JsonConvert.DeserializeObject<HourlyModel>(result1.ToString());
        WeakReferenceMessenger.Default.Send(new Message { Key = 101, Obj = HourlyModel });
    }

    private const string FileHourly = "data_hourly.json";

    private static void SaveDataHourly(string data)
    {
        SaveDataToJson(FileHourly, data);
    }

    private void LoadDataHourly()
    {
        var data = LoadDataFromJson(FileHourly);
        if (string.IsNullOrEmpty(data)) return;
        HourlyModel = JsonConvert.DeserializeObject<HourlyModel>(data);
    }

    #endregion

    #region 实时天气 Now

    public IRelayCommand CommandRenewNow { get; }

    private async void RequestNow(string apikey, string location)
    {
        var url =
            $"https://api.seniverse.com/v3/weather/now.json?key={apikey}&location={location}&language=zh-Hans&unit=c";
        var data = await _httpClient.GetStringAsync(url);
        ParseDataNow(data);
        SaveDataNow(data);
    }

    /// <summary>
    /// parse the data we want form the json
    /// </summary>
    /// <param name="json"></param>
    /// <returns>weather object</returns>
    private void ParseDataNow(string json)
    {
        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        if (result1 == null) return;
        NowModel = JsonConvert.DeserializeObject<NowModel>(result1.ToString());
    }

    private const string FileNow = "data_now.json";

    private static void SaveDataNow(string data)
    {
        SaveDataToJson(FileNow, data);
    }

    private NowModel _nowModel;

    public NowModel NowModel
    {
        get => _nowModel;
        private set => SetProperty(ref _nowModel, value);
    }

    private void LoadDataNow()
    {
        var data = LoadDataFromJson(FileNow);
        if (string.IsNullOrEmpty(data)) return;
        NowModel = JsonConvert.DeserializeObject<NowModel>(data);
    }

    #endregion

    #region 基础方法

    private void LoadSavedData()
    {
        LoadDataNow();
        LoadDataHourly();
        LoadDataDaily();
    }

    /// <summary>
    /// save data to json file
    /// </summary>
    /// <param name="filename">target file</param>
    /// <param name="jsondata">json data</param>
    private static void SaveDataToJson(string filename, string jsondata)
    {
        using Stream stream = File.Open(filename, FileMode.Create);
        using var sw = new StreamWriter(stream);
        sw.WriteLine(jsondata);
    }

    /// <summary>
    /// Load data from json
    /// </summary>
    private static string LoadDataFromJson(string filename)
    {
        if (!File.Exists(filename)) return null;
        try
        {
            using Stream stream = File.OpenRead(filename);
            using var sr = new StreamReader(stream);
            var json = sr.ReadToEnd();

            var jObject = JObject.Parse(json);
            var result0 = jObject["results"];
            var result1 = result0?.ToList().FirstOrDefault();
            return result1?.ToString();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return null;
        }
    }

    #endregion
}