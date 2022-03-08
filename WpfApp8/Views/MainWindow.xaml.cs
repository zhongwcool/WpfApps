using System;
using System.Windows;
using LibVLCSharp.Shared;
using WpfApp8.ViewModels;

namespace WpfApp8.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly LibVLC _libVlc;
    private const string Source0 = "http://playtv-live.ifeng.com:80/live/06OLEGEGM4G.m3u8";
    private const string Source1 = "http://playtv-live.ifeng.com:80/live/06OLEEWQKN4_tv1.m3u8";
    private const string Source2 = "http://111.63.117.13:6060/030000001000/CCTV-5/CCTV-5.m3u8";
    private const string Source3 = "http://183.207.249.138/ott.js.chinamobile.com/PLTV/3/224/3221227430/index.m3u8";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();

        _libVlc = new LibVLC();

        // we need the VideoView to be fully loaded before setting a MediaPlayer on it.
        VideoView0.Loaded += (_, _) =>
        {
            var mediaPlayer = new MediaPlayer(_libVlc);
            VideoView0.MediaPlayer = mediaPlayer;
            var media = new Media(_libVlc, new Uri(Source0));
            VideoView0.MediaPlayer.Play(media);
        };
        VideoView1.Loaded += (_, _) =>
        {
            var mediaPlayer = new MediaPlayer(_libVlc);
            VideoView1.MediaPlayer = mediaPlayer;
            var media = new Media(_libVlc, new Uri(Source1));
            VideoView1.MediaPlayer.Play(media);
        };
        VideoView2.Loaded += (_, _) =>
        {
            var mediaPlayer = new MediaPlayer(_libVlc);
            VideoView2.MediaPlayer = mediaPlayer;
            var media = new Media(_libVlc, new Uri(Source2));
            VideoView2.MediaPlayer.Play(media);
        };
        VideoView3.Loaded += (_, _) =>
        {
            var mediaPlayer = new MediaPlayer(_libVlc);
            VideoView3.MediaPlayer = mediaPlayer;
            var media = new Media(_libVlc, new Uri(Source2));
            VideoView3.MediaPlayer.Play(media);
        };

        BtnTest.Click += (s, e) => PopTest.IsOpen = true;
    }

    private void BtnTest_OnClick(object sender, RoutedEventArgs e)
    {
    }
}