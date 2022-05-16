using System.IO;
using System.Text;
using App10.Weather.Helper;
using App10.Weather.Models;
using Newtonsoft.Json;

namespace App10.Weather.Data;

public class AppConfig
{
    private static AppConfig _instance;

    public static AppConfig CreateInstance()
    {
        _instance ??= new AppConfig();
        return _instance;
    }

    private const string SectionTheme = "App Theme";
    private const string SectionOther = "Other";

    private static readonly string IniFile = Path.Combine(System.Environment.CurrentDirectory, @"App.ini");

    public string GetValue4Theme(string key)
    {
        return IniFileHelper.GetValue(SectionTheme, key, IniFile);
    }

    public bool SetValue4Theme(string key, string value)
    {
        return IniFileHelper.SetValue(SectionTheme, key, value, IniFile);
    }

    public string GetValue4Other(string key)
    {
        return IniFileHelper.GetValue(SectionOther, key, IniFile);
    }

    public bool SetValue4Other(string key, string value)
    {
        return IniFileHelper.SetValue(SectionOther, key, value, IniFile);
    }

    public static byte[] ToBytes()
    {
        if (null == _instance) return null;

        var myApp = new MyApp
        {
            AppTheme = _instance.GetValue4Theme("AppTheme"),
        };

        //序列化
        var json = JsonConvert.SerializeObject(myApp);

        return Encoding.UTF8.GetBytes(json);
    }
}