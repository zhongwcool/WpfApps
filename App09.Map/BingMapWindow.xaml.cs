using System.Windows;
using Microsoft.Maps.MapControl.WPF;

namespace App09.Map;

public partial class BingMapWindow : Window
{
    public BingMapWindow()
    {
        InitializeComponent();
        MyMap.Center = new Location(29.5617, 120.0962);
    }
}