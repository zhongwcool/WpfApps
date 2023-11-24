using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using App02.TTS.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mar.Cheese;
using Microsoft.CognitiveServices.Speech;

namespace App02.TTS.ViewModels;

public class MainViewModel : ObservableObject
{
    private SpeechConfig _config;

    public MainViewModel()
    {
        Prepare();

        CommandSpeak = new RelayCommand(() => { SpeakAsync(TxtContent); },
            () => !string.IsNullOrWhiteSpace(_selectedVoice));

        PropertyChanged += (_, args) =>
        {
            switch (args.PropertyName)
            {
                case nameof(SelectedVoice):
                {
                    CommandSpeak.NotifyCanExecuteChanged();
                    if (!string.IsNullOrWhiteSpace(SelectedVoice)) _config.SpeechSynthesisVoiceName = SelectedVoice;
                }
                    break;
            }
        };
    }

    private void Prepare()
    {
        Roles = new ObservableCollection<SpeechRole>(JsonUtil.Load<List<SpeechRole>>(JSON_ROLE));
        Styles = new ObservableCollection<SpeechStyle>(JsonUtil.Load<List<SpeechStyle>>(JSON_STYLE));
        Voices = new ObservableCollection<SpeechVoice>(JsonUtil.Load<List<SpeechVoice>>(JSON_VOICE));

        _styleItemsView = CollectionViewSource.GetDefaultView(Styles);
        _styleItemsView.Filter = CollectionFilter;

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
        SelectedItem = Voices.FirstOrDefault(voice => voice.VoiceKey == SelectedVoice);
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

    private SpeechVoice _selectedItem;

    public SpeechVoice SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetProperty(ref _selectedItem, value)) _styleItemsView.Refresh();
        }
    }

    private ICollectionView _styleItemsView;

    private string _selectedStyle = string.Empty;

    public string SelectedStyle
    {
        get => _selectedStyle;
        set => SetProperty(ref _selectedStyle, value);
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

        var ssml =
            @$"<speak version='1.0' xml:lang='zh-CN' xmlns='http://www.w3.org/2001/10/synthesis' xmlns:mstts='http://www.w3.org/2001/mstts'>
            <voice name='{_config.SpeechSynthesisVoiceName}'>
                <mstts:express-as style='{SelectedStyle}'>{text}</mstts:express-as>
            </voice>
        </speak>";

        // Required for sentence-level WordBoundary events
        _config.SetProperty(PropertyId.SpeechServiceResponse_RequestSentenceBoundary, "true");

        using var synthesizer = new SpeechSynthesizer(_config);
        using var result = await synthesizer.SpeakSsmlAsync(ssml);
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
        var item = (SpeechStyle)obj;
        if (string.IsNullOrWhiteSpace(item.StyleKey)) return true;
        if (SelectedItem == null) return false;
        return SelectedItem.SpeechStyles.Contains(item.StyleKey);
    }

    public IRelayCommand CommandSpeak { get; }

    public ObservableCollection<SpeechVoice> Voices { get; private set; }
    public ObservableCollection<SpeechStyle> Styles { get; private set; }
    public ObservableCollection<SpeechRole> Roles { get; private set; }
}