using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App01.VLC.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App01.VLC.ViewModels;

public class MainViewModel : ObservableObject
{
    public async Task LoadDataAsync()
    {
        var client = new HttpClient();
        const string playlistUrl = "https://raw.githubusercontent.com/joevess/IPTV/main/iptv.m3u8";
        var m3UContent = await client.GetStringAsync(playlistUrl);

        var channels = ParseM3UContent(m3UContent);

        foreach (var channel in channels)
        {
            Channels.Add(channel);
        }

        IsBusy = false;
    }

    private bool _isBusy = true;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
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
}