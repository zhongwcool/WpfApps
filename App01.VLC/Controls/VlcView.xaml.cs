using System;
using System.Threading.Tasks;
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

        VideoView.Loaded += async (_, _) =>
        {
            if (DataContext is not Channel channel) return;
            _libVlc = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);
            _mediaPlayer.Playing += (_, _) =>
            {
                Application.Current.Dispatcher.Invoke(() => { LoadView.Visibility = Visibility.Collapsed; });
            };

            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() => { VideoView.MediaPlayer = _mediaPlayer; });

                using var media = new Media(_libVlc, new Uri(channel.Url));
                _mediaPlayer.Play(media);
            });
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