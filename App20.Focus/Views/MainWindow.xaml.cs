using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Mar.Controls.Tool;
using Serilog;

namespace App20.Focus.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Task.Delay(500).ContinueWith(_ =>
        {
            Dispatcher.Invoke(() =>
            {
                var console = new ConsoleWindow(this)
                {
                    PrintHello = false
                };
                console.Show();
            });
        });
    }

    private void MyControl_MouseEnter(object sender, MouseEventArgs e)
    {
        Log.Debug("mouse enter");
        // 创建关键帧动画
        var animation = new DoubleAnimationUsingKeyFrames
        {
            Duration = TimeSpan.FromSeconds(2.5),
            RepeatBehavior = RepeatBehavior.Forever
        };

        // 定义透明度关键帧
        DoubleKeyFrame keyFrame1 = new LinearDoubleKeyFrame(1, TimeSpan.FromSeconds(0));
        DoubleKeyFrame keyFrame2 = new LinearDoubleKeyFrame(0.2, TimeSpan.FromSeconds(1));
        DoubleKeyFrame keyFrame3 = new LinearDoubleKeyFrame(1, TimeSpan.FromSeconds(2));

        // 将关键帧添加到动画中
        animation.KeyFrames.Add(keyFrame1);
        animation.KeyFrames.Add(keyFrame2);
        animation.KeyFrames.Add(keyFrame3);

        // 应用动画到控件的透明度属性
        MyControl.BeginAnimation(OpacityProperty, animation);
    }

    private void MyControl_MouseLeave(object sender, MouseEventArgs e)
    {
        Log.Debug("mouse leave");
        // 停止动画
        MyControl.BeginAnimation(OpacityProperty, null);
    }
}