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
    public MainViewModel()
    {
        Prepare();

        CommandSpeak = new RelayCommand(() => { SpeakAsync(TxtContent); });
    }

    private void Prepare()
    {
        var model0 = JsonUtil.Load<RoleModel>(JSON_ROLES);
        if (model0 == null) return;
        foreach (var role in model0.Roles) Roles.Add(role);

        var model = JsonUtil.Load<AzureModel>(JSON_AZURE);
        if (model == null) return;
        var azure = model.Azure;

        //
        // For more samples please visit https://github.com/Azure-Samples/cognitive-services-speech-sdk 
        // 
        _config = SpeechConfig.FromSubscription(azure.SubscriptionKey, azure.SubscriptionRegion);
        TxtContent = azure.Text;

        for (var i = 0; i < Roles.Count; i++)
        {
            if (Roles[i].VoiceKey != azure.VoiceKey) continue;
            IndexRole = i;
            break;
        }

        if (-1 == IndexRole)
        {
            IndexRole = 0;
            SelectedValue = Roles[IndexRole].VoiceKey;
        }
        else
        {
            SelectedValue = Roles[IndexRole].VoiceKey;
        }

        _config.SpeechSynthesisVoiceName = SelectedValue;
    }

    private const string JSON_AZURE = "azure.json";
    private const string JSON_ROLES = "roles.json";

    private SpeechConfig _config;

    private string _selectedValue = string.Empty;

    public string SelectedValue
    {
        get => _selectedValue;
        set
        {
            SetProperty(ref _selectedValue, value);
            _config.SpeechSynthesisVoiceName = _selectedValue;
        }
    }

    public int IndexRole { get; set; } = -1;
    public string TxtError { get; set; } = string.Empty;
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

    public ObservableCollection<Role> Roles { get; set; } = new();
}