using System;
using System.Diagnostics;
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
    private readonly HttpClient _httpClient = new();

    private MainViewModel()
    {
        CommandRenewNow = new RelayCommand(() => { RequestNow("31.401973:120.736730"); });

        LoadDataNow();
    }

    public static MainViewModel CreateInstance()
    {
        _instance ??= new MainViewModel();
        return _instance;
    }

    #region 实时天气 Now

    public IRelayCommand CommandRenewNow { get; }

    private async void RequestNow(string location)
    {
        var url =
            $"https://api.seniverse.com/v3/weather/now.json?key=SonT2HaJQRjGe-C_E&location={location}&language=zh-Hans&unit=c";
        var data = await _httpClient.GetStringAsync(url);
        ParseJsonDataNow(data);
        SaveDataNow(data);
    }

    /// <summary>
    /// parse the data we want form the json
    /// </summary>
    /// <param name="json"></param>
    /// <returns>weather object</returns>
    private void ParseJsonDataNow(string json)
    {
        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        if (result1 == null) return;
        NowModel = JsonConvert.DeserializeObject<NowModel>(result1.ToString());
    }

    private const string FileNow = "now.json";

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