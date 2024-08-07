﻿namespace App21.Skeleton.Models;

public class MyApp
{
    public string Site { get; set; } =
        "https://mirror.ghproxy.com/https://raw.githubusercontent.com/joevess/IPTV/main/iptv.m3u8";

    public List<Channel> Channels { get; set; } = [];
    public DateTime LastModified { get; set; } = DateTime.Now;
}