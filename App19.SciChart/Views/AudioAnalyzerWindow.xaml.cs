using System.Windows;
using App19.SciChart.ViewModels;

namespace App19.SciChart.Views;

public partial class AudioAnalyzerWindow : Window
{
    public AudioAnalyzerWindow()
    {
        InitializeComponent();
        DataContext = new AudioAnalyzerViewModel();
    }
}