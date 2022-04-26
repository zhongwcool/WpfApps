using System.Windows;
using LibVLCSharp.Shared;

namespace App08.Metro;

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