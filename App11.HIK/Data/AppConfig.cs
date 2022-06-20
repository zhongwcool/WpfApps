using System;
using System.IO;
using App11.HIK.Helper;

namespace App11.HIK.Data;

public class AppConfig
{
    public string UserName
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "UserName");
            return port;
        }
        set => _instance.SetValue(Section.HikVision, "UserName", value);
    }

    public string Password
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "Password");
            return port;
        }
        set => _instance.SetValue(Section.HikVision, "Password", value);
    }

    public short Port
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "Port");
            return string.IsNullOrEmpty(port) ? Convert.ToInt16(8000) : short.Parse(port);
        }
        set => _instance.SetValue(Section.HikVision, "Port", value.ToString());
    }

    public string IPAddress
    {
        get
        {
            var port = _instance.GetValue(Section.HikVision, "IPAddress");
            return port;
        }
        set => _instance.SetValue(Section.HikVision, "IPAddress", value);
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