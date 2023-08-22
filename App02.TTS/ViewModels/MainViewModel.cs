using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
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
            if (args.PropertyName == nameof(SelectedVoice))
            {
                CommandSpeak.NotifyCanExecuteChanged();
                if (!string.IsNullOrWhiteSpace(SelectedVoice)) _config.SpeechSynthesisVoiceName = SelectedVoice;
            }
        };

        CommandSpeak = new RelayCommand(() => { SpeakAsync(TxtContent); },
            () => !string.IsNullOrWhiteSpace(_selectedVoice));
    }

    private void Prepare()
    {
        Roles = new ObservableCollection<SpeechRole>(JsonUtil.Load<List<SpeechRole>>(JSON_ROLE));
        Styles = new ObservableCollection<SpeechStyle>(JsonUtil.Load<List<SpeechStyle>>(JSON_STYLE));
        Voices = new ObservableCollection<SpeechVoice>(JsonUtil.Load<List<SpeechVoice>>(JSON_VOICE));

        _voiceItemsView = CollectionViewSource.GetDefaultView(Voices);
        _voiceItemsView.Filter = CollectionFilter;

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
        SelectedVoice = azure.VoiceKey;
        _config.SpeechSynthesisVoiceName = SelectedVoice;
    }

    private const string JSON_AZURE = "azure.json";
    private const string JSON_VOICE = "voices.json";
    private const string JSON_STYLE = "styles.json";
    private const string JSON_ROLE = "roles.json";

    private string _selectedVoice = string.Empty;

    public string SelectedVoice
    {
        get => _selectedVoice;
        set => SetProperty(ref _selectedVoice, value);
    }

    private ICollectionView _voiceItemsView;

    private string _selectedStyle = string.Empty;

    public string SelectedStyle
    {
        get => _selectedStyle;
        set
        {
            if (SetProperty(ref _selectedStyle, value)) _voiceItemsView.Refresh();
        }
    }

    private string _txtError = string.Empty;

    public string TxtError
    {
        get => _txtError;
        private set => SetProperty(ref _txtError, value);
    }
    public string TxtContent { get; set; } = string.Empty;

    private async void SpeakAsync(string text)
    {
        if (null == _config) return;
        using var synthesizer = new SpeechSynthesizer(_config);
        using var result = await synthesizer.SpeakTextAsync(text);
        switch (result.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                TxtError = $"Done. [{text}]";
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

    private bool CollectionFilter(object obj)
    {
        if (string.IsNullOrWhiteSpace(SelectedStyle)) return true;

        var item = (SpeechVoice)obj;
        return item.SpeechStyles.Contains(SelectedStyle);
    }

    public IRelayCommand CommandSpeak { get; }

    public ObservableCollection<SpeechVoice> Voices { get; private set; }
    public ObservableCollection<SpeechStyle> Styles { get; private set; }
    public ObservableCollection<SpeechRole> Roles { get; private set; }
}