using System.Windows;
using App19.SciChart00.ViewModels;

namespace App19.SciChart00.Views;

public partial class AudioAnalyzerWindow : Window
{
    public AudioAnalyzerWindow()
    {
        InitializeComponent();
        DataContext = new AudioAnalyzerViewModel();
    }
}