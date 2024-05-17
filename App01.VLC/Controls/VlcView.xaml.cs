using System;
using System.Windows;
using App01.VLC.Models;
using LibVLCSharp.Shared;

namespace App01.VLC.Controls;

public partial class VlcView
{
    private LibVLC _libVlc;
    private MediaPlayer _mediaPlayer;

    public VlcView()
    {
        InitializeComponent();

        VideoView.Loaded += (_, _) =>
        {
            if (DataContext is not Channel channel) return;
            _libVlc = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);

            VideoView.MediaPlayer = _mediaPlayer;
            using var media = new Media(_libVlc, new Uri(channel.Url));
            VideoView.MediaPlayer.Play(media);
        };
        Unloaded += (_, _) =>
        {
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _libVlc?.Dispose();
        };
    }

    // 设备点击事件
    private void OnTitleClick(object sender, RoutedEventArgs e)
    {
    }
}