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

    private static readonly string IniFile = Path.Combine(System.Environment.CurrentDirectory, @"App.ini");

    public string GetValue(string section, string key)
    {
        return IniFileHelper.GetValue(section, key, IniFile);
    }

    public bool SetValue(string section, string key, string value)
    {
        return IniFileHelper.SetValue(section, key, value, IniFile);
    }

    public static byte[] ToBytes()
    {
        if (null == _instance) return null;

        var myApp = new MyApp
        {
            AppTheme = _instance.GetValue(Section.Theme, "AppTheme"),
        };

        //序列化
        var json = JsonConvert.SerializeObject(myApp);

        return Encoding.UTF8.GetBytes(json);
    }
}

public class Section
{
    public const string Theme = "App Theme";
    public const string Other = "Other";
}