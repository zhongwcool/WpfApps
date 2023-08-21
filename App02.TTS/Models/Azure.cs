namespace App02.TTS.Models;

public class Azure
{
    public string SubscriptionKey { get; set; } = string.Empty;
    public string SubscriptionRegion { get; set; } = string.Empty;
    public string VoiceKey { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}