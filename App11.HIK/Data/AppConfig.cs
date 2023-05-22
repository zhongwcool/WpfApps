using System;
using System.IO;
using App11.HIK.Helper;

namespace App11.HIK.Data;

public class AppConfig
{
    public string HikUserName
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "HikUserName");
            return port;
        }
        set => _instance.SetValue(Section.HikVision, "HikUserName", value);
    }

    public string HikPassword
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "HikPassword");
            return port;
        }
        set => _instance.SetValue(Section.HikVision, "HikPassword", value);
    }

    public short HikPort
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "HikPort");
            return string.IsNullOrEmpty(port) ? Convert.ToInt16(8000) : short.Parse(port);
        }
        set => _instance.SetValue(Section.HikVision, "HikPort", value.ToString());
    }

    public string HikDemoIp
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "HikDemoIp");
            return port;
        }
        set => _instance.SetValue(Section.HikVision, "HikDemoIp", value);
    }

    private static AppConfig _instance;

    public static AppConfig CreateInstance()
    {
        _instance ??= new AppConfig();
        return _instance;
    }

    #region 基础方法

    private static readonly string IniFile = Path.Combine(Environment.CurrentDirectory, @"App.ini");

    public string GetValue(string section, string key)
    {
        return IniFileHelper.GetValue(section, key, IniFile);
    }

    public bool SetValue(string section, string key, string value)
    {
        return IniFileHelper.SetValue(section, key, value, IniFile);
    }

    #endregion
}

public class Section
{
    public const string Theme = "App Theme";
    public const string HikVision = "HikVision";
}