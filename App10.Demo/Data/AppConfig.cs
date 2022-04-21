using System.IO;
using System.Text;
using App10.Demo.Helper;
using App10.Demo.Model;
using Newtonsoft.Json;

namespace App10.Demo.Data;

public class AppConfig
{
    private static AppConfig _instance;

    public static AppConfig CreateInstance()
    {
        _instance ??= new AppConfig();
        return _instance;
    }

    private const string SectionName = "App Theme";

    private static readonly string IniFile = Path.Combine(System.Environment.CurrentDirectory, @"App.ini");

    public string GetValue(string key)
    {
        return IniFileHelper.GetValue(SectionName, key, IniFile);
    }

    public bool SetValue(string key, string value)
    {
        return IniFileHelper.SetValue(SectionName, key, value, IniFile);
    }

    public static byte[] ToBytes()
    {
        if (null == _instance) return null;

        var myApp = new MyApp
        {
            AppTheme = _instance.GetValue("AppTheme"),
        };

        //序列化
        var json = JsonConvert.SerializeObject(myApp);

        return Encoding.UTF8.GetBytes(json);
    }
}