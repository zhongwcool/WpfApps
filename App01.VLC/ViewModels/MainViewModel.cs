using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using App01.VLC.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Mar.Cheese;
using Channel = App01.VLC.Models.Channel;

namespace App01.VLC.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Prepare();

        _semaphore = new SemaphoreSlim(8, 10);
        _client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        Channels.CollectionChanged += (sender, args) =>
        {
            //当数量超过1时将IsBusy设为false
            if (Channels.Count > 1) return;
            IsBusy = false;
            ShowPanel = true;
        };

        _channelItemsView = CollectionViewSource.GetDefaultView(Channels);
        _channelItemsView.Filter = CollectionFilter;
    }

    private void Prepare()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var assembly = Assembly.GetEntryAssembly()?.GetName();
        var myAppFolder = Path.Combine(appDataPath,
            null == assembly || string.IsNullOrEmpty(assembly.Name) ? "DouTV" : assembly.Name);
        var jsonFile = Path.Combine(myAppFolder, JSON_FILE);

        try
        {
            var model = JsonUtil.Load<MyApp>(jsonFile);
            if (model == null) return;
            _config = model;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async void UpdateSites(string newSite)
    {
        if (_config.Site == newSite) return;
        _config.Channels.Clear();
        await LoadDataAsync();
    }

    private MyApp _config;

    private const string JSON_FILE = "app.json";

    private readonly HttpClient _client;
    private readonly SemaphoreSlim _semaphore;

    public async Task LoadDataAsync(bool useLocal = false)
    {
        Channels.Clear();
        Groups.Clear();
        IsBusy = true;
        TxtStatus = "加载播放数据..";

        if (useLocal && _config.Channels.Count > 0)
        {
            foreach (var channel in _config.Channels)
            {
                Channels.Add(channel);
                //将channel中的GroupTitle添加到Groups中
                if (Groups.All(gp => gp.Title != channel.GroupTitle))
                {
                    Groups.Add(new TvGroup { Title = channel.GroupTitle });
                }
            }

            SelectedGroup = Groups.FirstOrDefault();
            return;
        }

        try
        {
            var m3UContent = await _client.GetStringAsync(_config.Site);
            var channels = ParseM3UContent(m3UContent);
            Max = channels.Count;
            TxtStatus = "解析数据..";
            var counter = 0;
            foreach (var channel in channels)
            {
                _ = Task.Run(async () =>
                {
                    await _semaphore.WaitAsync();
                    var isAvailable = await CheckIptvUrl(channel);
                    if (isAvailable)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Channels.Add(channel);
                            //将channel中的GroupTitle添加到Groups中
                            if (Groups.All(gp => gp.Title != channel.GroupTitle))
                            {
                                Groups.Add(new TvGroup { Title = channel.GroupTitle });
                            }

                            IsBusy2 = true;
                        });
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Interlocked.Increment(ref counter);
                        Index = counter;
                        TxtStatus = $"检查频道是否有效..{Index}/{Max}";
                        if (Index == Max)
                        {
                            IsBusy2 = false;
                            SelectedGroup = Groups.FirstOrDefault();
                            SaveUserChoice();
                        }
                    });
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TxtStatus = $"!更新数据出错..请稍后重试";
        }
    }

    // 保存用户选择
    private void SaveUserChoice()
    {
        _config.Channels.Clear();
        _config.Channels.AddRange(Channels);

        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var assembly = Assembly.GetEntryAssembly()?.GetName();
        var myAppFolder = Path.Combine(appDataPath,
            null == assembly || string.IsNullOrEmpty(assembly.Name) ? "DouTV" : assembly.Name);
        Directory.CreateDirectory(myAppFolder);
        var jsonFile = Path.Combine(myAppFolder, JSON_FILE);
        _config.LastModified = DateTime.Now;
        JsonUtil.Save(jsonFile, _config);
    }

    private bool _isBusy = true;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private bool _isBusy2 = false;

    public bool IsBusy2
    {
        get => _isBusy2;
        set => SetProperty(ref _isBusy2, value);
    }

    private bool _showPanel = false;

    public bool ShowPanel
    {
        get => _showPanel;
        set => SetProperty(ref _showPanel, value);
    }

    private string _txtStatus = "Data is preparing..";

    public string TxtStatus
    {
        get => _txtStatus;
        set => SetProperty(ref _txtStatus, value);
    }

    private int _index = 0;

    public int Index
    {
        get => _index;
        set => SetProperty(ref _index, value);
    }

    private int _max = 100;

    public int Max
    {
        get => _max;
        set => SetProperty(ref _max, value);
    }

    public ObservableCollection<Channel> Channels { get; set; } = [];

    private static List<Channel> ParseM3UContent(string m3UContent)
    {
        List<Channel> channels = [];
        var lines = m3UContent.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (!line.StartsWith("#EXTINF:")) continue;
            var channel = new Channel();
            // 解析属性
            var match = Regex.Match(line, @"group-title=""([^""]+)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                channel.GroupTitle = match.Groups[1].Value;
            }

            match = Regex.Match(line, @"tvg-id=""([^""]+)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                channel.TvgId = match.Groups[1].Value;
            }

            match = Regex.Match(line, @"tvg-logo=""([^""]+)""", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                channel.TvgLogo = match.Groups[1].Value;
            }

            match = Regex.Match(line, @",(.*?)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                channel.Name = match.Groups[1].Value;
            }

            // Assume the line immediately following "#EXTINF:" is the URL of the channel
            if (i + 1 >= lines.Length || lines[i + 1].StartsWith($"#")) continue;
            channel.Url = lines[i + 1].Trim();
            channels.Add(channel);
        }

        return channels;
    }

    private async Task<bool> CheckIptvUrl(Channel channel)
    {
        try
        {
            var response = await _client.GetAsync(channel.Url, HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode &&
                   response.Content.Headers.ContentType?.MediaType == "application/vnd.apple.mpegurl";
        }
        catch (Exception e)
        {
            "Exception Caught!".PrintErr();
            $"Message :{e.Message} ".PrintErr();
        }
        finally
        {
            // 释放 SemaphoreSlim
            _semaphore.Release();
        }

        return false;
    }

    private bool CollectionFilter(object obj)
    {
        if (SelectedGroup == null) return true;
        if (SelectedGroup.Title == "全部") return true;
        var channel = (Channel)obj;
        return Channels.Contains(channel) && channel.GroupTitle == SelectedGroup.Title;
    }

    private readonly ICollectionView _channelItemsView;

    private TvGroup _selectedGroup;

    public TvGroup SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            if (SetProperty(ref _selectedGroup, value)) _channelItemsView.Refresh();
        }
    }

    public ObservableCollection<TvGroup> Groups { get; private set; } = [];
}