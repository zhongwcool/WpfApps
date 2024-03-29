﻿using System;
using System.IO;
using App04.MultiLang.Helper;

namespace App04.MultiLang.Config;

public class LanguageConfig
{
    public static readonly LanguageConfig Instance;

    static LanguageConfig()
    {
        Instance = new LanguageConfig();
    }

    private static readonly string IniFilePath = Path.Combine(Environment.CurrentDirectory, @"Config.ini");

    private string _languageCurrent = IniFileHelper.GetValue("Common", "Language", IniFilePath);

    public string LanguageCurrent
    {
        get => _languageCurrent;

        set
        {
            _languageCurrent = value;
            IniFileHelper.SetValue("Common", "Language", value, IniFilePath);
        }
    }
}