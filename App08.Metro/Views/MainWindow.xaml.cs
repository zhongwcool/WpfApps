using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using App08.Metro.ViewModels;
using LibVLCSharp.Shared;

namespace App08.Metro.Views;

public partial class MainWindow : Window
{
    private const string Source0 = "http://playtv-live.ifeng.com:80/live/06OLEGEGM4G.m3u8";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();

        var libVlc = new LibVLC();

        // we need the VideoView to be fully loaded before setting a MediaPlayer on it.
        VideoView.Loaded += (_, _) =>
        {
            var mediaPlayer = new MediaPlayer(libVlc);
            VideoView.MediaPlayer = mediaPlayer;
            var media = new Media(libVlc, new Uri(Source0));
            VideoView.MediaPlayer.Play(media);
        };
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        XuNiBox.Focus();
        var sb = Resources["CloseMenu"] as Storyboard;
        sb?.Begin(RightMenu);
    }

    private void BtnTest_OnClick(object sender, RoutedEventArgs e)
    {
        var sb = Resources["OpenMenu"] as Storyboard;
        sb?.Begin(RightMenu);
    }
}