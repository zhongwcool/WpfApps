using System.Diagnostics;
using Windows.Data.Json;
using Windows.UI.Notifications;

namespace App11.HIK.Utils;

public static class AppUtil
{
    /// <summary>
    /// Look up a string in the saved LocalSettings, or a default value if no string is found.
    /// </summary>
    /// <param name="key">The key to look up</param>
    /// <param name="defaultValue">The fallback value to use if the key is not found or the value is not a string</param>
    /// <returns></returns>
    public static string LookupSavedString(string key, string defaultValue = "")
    {
        var values = Windows.Storage.ApplicationData.Current.LocalSettings.Values;
        values.TryGetValue(key, out var o);
        var s = o as string;
        return s ?? defaultValue;
    }

    /// <summary>
    /// Looks up a JsonArray from the saved LocalSettings, or an empty JsonArray if not found.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static JsonArray LoadSavedJson(string key)
    {
        var json = LookupSavedString(key);
        if (string.IsNullOrEmpty(json))
        {
            // Not present. Return an empty array.
            return new JsonArray();
        }
        else
        {
            return JsonValue.Parse(json).GetArray();
        }
    }

    /// <summary>
    /// Helper method to display a toast notification.
    /// </summary>
    public static void ShowToast(string body, string title = "")
    {
        var p = Process.GetCurrentProcess();
        var toastNotifier = ToastNotificationManager.CreateToastNotifier(p.ProcessName);

        // Create a two line toast and add audio reminder

        // Here the xml that will be passed to the
        // ToastNotification for the toast is retrieved
        var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

        // Set both lines of text
        var toastNodeList = toastXml.GetElementsByTagName("text");
        toastNodeList.Item(0)
            ?.AppendChild(toastXml.CreateTextNode(string.IsNullOrEmpty(title) ? p.ProcessName : title));
        toastNodeList.Item(1)?.AppendChild(toastXml.CreateTextNode(body));

        // now create a xml node for the audio source
        var toastNode = toastXml.SelectSingleNode("/toast");
        var audio = toastXml.CreateElement("audio");
        audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

        var toast = new ToastNotification(toastXml);
        toastNotifier.Show(toast);
    }
}