using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace App23.BottomSheet.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowBottomSheet_Click(object sender, RoutedEventArgs e)
    {
        // Show the Overlay
        Overlay.Visibility = Visibility.Visible;

        // Animate the Bottom Sheet to slide up
        var animation = new DoubleAnimationUsingKeyFrames();
        animation.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
            new KeySpline(0.2, 0.8, 0.3, 1.0)));

        BottomSheet.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);
    }

    private void HideBottomSheet_Click(object sender, RoutedEventArgs e)
    {
        HideBottomSheet();
    }

    private void Overlay_MouseDown(object sender, RoutedEventArgs e)
    {
        HideBottomSheet();
    }

    private void HideBottomSheet()
    {
        // Animate the Bottom Sheet to slide down
        var animation = new DoubleAnimationUsingKeyFrames();
        animation.KeyFrames.Add(new SplineDoubleKeyFrame(200, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
            new KeySpline(0.8, 0.0, 0.9, 0.2)));

        animation.Completed += (s, e) =>
        {
            // Hide the Overlay when the animation finishes
            Overlay.Visibility = Visibility.Collapsed;
        };

        BottomSheet.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);
    }
}