﻿using System.Windows;
using LibVLCSharp.Shared;

namespace WpfApp8;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Core.Initialize();
    }
}