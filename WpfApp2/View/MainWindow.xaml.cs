using System;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.View;

public partial class MainWindow : Window
{
    private readonly SpeechSynthesizer _speechSynthesizer = new();
    private const string VoiceName = "Microsoft Huihui Desktop";

    public MainWindow()
    {
        InitializeComponent();

        //TODO 填充语言选项
        var voiceInstalled = _speechSynthesizer.GetInstalledVoices().FirstOrDefault(
            x => x.VoiceInfo.Description.Contains(VoiceName)
        ) != null;

        if (!voiceInstalled) MessageBox.Show($"Voice '{VoiceName}' is not installed in system. Now we exit");
        _speechSynthesizer.SelectVoice(VoiceName);
    }

    private void VolumeChanged(object sender, RoutedEventArgs args)
    {
        _speechSynthesizer.Volume = (int)((Slider)args.OriginalSource).Value;
    }

    private void RateChanged(object sender, RoutedEventArgs args)
    {
        _speechSynthesizer.Rate = (int)((Slider)args.OriginalSource).Value;
    }

    private void ButtonEchoOnClick(object sender, RoutedEventArgs args)
    {
        _speechSynthesizer.SpeakAsync(TextToDisplay.Text);
    }

    private void ButtonDateOnClick(object sender, RoutedEventArgs args)
    {
        _speechSynthesizer.SpeakAsync("Today is " + DateTime.Now.ToShortDateString());
    }

    private void ButtonTimeOnClick(object sender, RoutedEventArgs args)
    {
        _speechSynthesizer.SpeakAsync("The time is " + DateTime.Now.ToShortTimeString());
    }

    private void ButtonNameOnClick(object sender, RoutedEventArgs args)
    {
        _speechSynthesizer.SpeakAsync("My name is " + _speechSynthesizer.Voice.Name);
    }
}