using System;
using System.Windows;
using LibVLCSharp.Shared;

namespace WpfApp1
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LibVLC _libVlc;
        private readonly MediaPlayer _mediaPlayer;

        public MainWindow()
        {
            InitializeComponent();

            _libVlc = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);

            // we need the VideoView to be fully loaded before setting a MediaPlayer on it.
            VideoView.Loaded += (_, _) => VideoView.MediaPlayer = _mediaPlayer;
            Unloaded += VideoView_Unloaded;
        }

        private void VideoView_Unloaded(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVlc.Dispose();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var mediaPlay = VideoView.MediaPlayer;
            if (null == mediaPlay) return;
            if (mediaPlay.IsPlaying) return;
            //using var media = new Media(_libVlc, new Uri("rtmp://192.168.7.239:1935/live/stream"));
            using var media = new Media(_libVlc, new Uri("http://zbbf1.ahtv.cn/live/756.flv"));
            mediaPlay.Play(media);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView.MediaPlayer is { IsPlaying: true }) VideoView.MediaPlayer.Stop();
        }

        protected override void OnClosed(EventArgs e)
        {
            VideoView.Dispose();
        }
    }
}