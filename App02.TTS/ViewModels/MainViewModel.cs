using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using App02.TTS.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mar.Console;
using Microsoft.CognitiveServices.Speech;

namespace App02.TTS.ViewModels;

public class MainViewModel : ObservableObject
{
    private SpeechConfig _config;
    
    public MainViewModel()
    {
        Prepare();
        PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(SelectedValue)) _config.SpeechSynthesisVoiceName = SelectedValue;
        };

        CommandSpeak = new RelayCommand(() => { SpeakAsync(TxtContent); });
    }

    private void Prepare()
    {
        Styles = new ObservableCollection<SpeechStyle>(JsonUtil.Load<List<SpeechStyle>>(JSON_STYLE));
        Voices = new ObservableCollection<SpeechVoice>(JsonUtil.Load<List<SpeechVoice>>(JSON_VOICE));

        var azure = JsonUtil.Load<Models.Azure>(JSON_AZURE);
        if (azure == null)
        {
            TxtError = "Azure.json is not found!";
            return;
        }
        //
        // For more samples please visit https://github.com/Azure-Samples/cognitive-services-speech-sdk 
        // 
        _config = SpeechConfig.FromSubscription(azure.SubscriptionKey, azure.SubscriptionRegion);
        TxtContent = azure.Text;
        SelectedValue = azure.VoiceKey;
        _config.SpeechSynthesisVoiceName = SelectedValue;
    }

    private const string JSON_AZURE = "azure.json";
    private const string JSON_VOICE = "voices.json";
    private const string JSON_STYLE = "styles.json";

    private string _selectedValue = string.Empty;

    public string SelectedValue
    {
        get => _selectedValue;
        set => SetProperty(ref _selectedValue, value);
    }

    public string TxtError { get; private set; } = string.Empty;
    public string TxtContent { get; set; } = string.Empty;

    private async void SpeakAsync(string text)
    {
        if (null == _config) return;
        using var synthesizer = new SpeechSynthesizer(_config);
        using var result = await synthesizer.SpeakTextAsync(text);
        switch (result.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                TxtError = $"Speech synthesized for text [{text}]";
                break;
            case ResultReason.Canceled:
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                TxtError = $"CANCELED: Reason={cancellation.Reason}";

                if (cancellation.Reason == CancellationReason.Error)
                {
                    var builder = new StringBuilder();
                    builder.Append($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    builder.Append($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    builder.Append("CANCELED: Did you update the subscription info?");
                    TxtError = builder.ToString();
                }
            }
                break;
        }
    }

    public IRelayCommand CommandSpeak { get; }

    public ObservableCollection<SpeechVoice> Voices { get; set; }
    public ObservableCollection<SpeechStyle> Styles { get; set; }
}