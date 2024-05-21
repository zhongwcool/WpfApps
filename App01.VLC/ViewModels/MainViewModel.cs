using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using App01.VLC.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Mar.Cheese;

namespace App01.VLC.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        _semaphore = new SemaphoreSlim(8, 10);
        _client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        Channels.CollectionChanged += (sender, args) =>
        {
            //当数量超过1时将IsBusy设为false
            if (Channels.Count <= 1)
            {
                IsBusy = false;
                IsBusy2 = true;
            }
        };
    }

    private readonly HttpClient _client;
    private readonly SemaphoreSlim _semaphore;

    public async Task LoadDataAsync()
    {
        const string playlistUrl =
            "https://mirror.ghproxy.com/https://raw.githubusercontent.com/joevess/IPTV/main/iptv.m3u8";
        TxtStatus = "加载播放数据..";
        var m3UContent = await _client.GetStringAsync(playlistUrl);
        var channels = ParseM3UContent(m3UContent);
        Max = channels.Count;

        TxtStatus = "检出有效地址..";
        var counter = 0;
        foreach (var channel in channels)
        {
            _ = Task.Run(async () =>
            {
                await _semaphore.WaitAsync();
                var isAvailable = await CheckIptvUrl(channel);
                if (isAvailable)
                {
                    Application.Current.Dispatcher.Invoke(() => { Channels.Add(channel); });
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Interlocked.Increment(ref counter);
                    Index = counter;
                    TxtStatus = $"检出有效地址..{Index}/{Max}";
                    if (Index == Max)
                    {
                        IsBusy2 = false;
                    }
                });
            });
        }
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
}