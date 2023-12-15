using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Mar.Controls.Tool;
using Serilog;

namespace App20.Focus.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;

        Task.Delay(500).ContinueWith(_ =>
        {
            Dispatcher.Invoke(() =>
            {
                var console = new ConsoleWindow(this)
                {
                    PrintEnv = false
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

    private readonly Random _random = new();
    private readonly List<Ellipse> _particles = new();
    private readonly DispatcherTimer _timer = new();
    private readonly DispatcherTimer _particleGenerator = new();
    private const double Gravity = 98;
    private const double MaxHorizontalDistance = 200; // 水平方向的最大距离
    private const double MaxVerticalDistance = 100; // 竖直方向的最大距离

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _timer.Interval = TimeSpan.FromMilliseconds(20);
        _timer.Tick += Timer_Tick;
        _timer.Start();

        _particleGenerator.Interval = TimeSpan.FromMilliseconds(20);
        _particleGenerator.Tick += ParticleGenerator_Tick;
        _particleGenerator.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        foreach (var particle in _particles.ToArray())
        {
            var time = (DateTime.Now - (DateTime)particle.Tag).TotalSeconds;
            var (initialVelocity, angle) = ((double, double))particle.DataContext;

            var radian = angle * Math.PI / 180;
            var horizontalVelocity = initialVelocity * Math.Cos(radian);
            var verticalVelocity = initialVelocity * Math.Sin(radian);
            var x = horizontalVelocity * time;
            var y = verticalVelocity * time - 0.5 * Gravity * time * time;

            if (Math.Abs(x) > MaxHorizontalDistance || y < -MaxVerticalDistance)
            {
                ParticleCanvas.Children.Remove(particle);
                _particles.Remove(particle);
            }
            else
            {
                Canvas.SetLeft(particle, x + ParticleCanvas.ActualWidth / 2);
                Canvas.SetTop(particle, ParticleCanvas.ActualHeight - y);
            }
        }
    }

    private void ParticleGenerator_Tick(object sender, EventArgs e)
    {
        CreateParticle();
    }

    private void CreateParticle()
    {
        var ellipse = new Ellipse
        {
            Fill = Brushes.Gold,
            Width = 5,
            Height = 5,
            DataContext = (InitialVelocity(), RandomAngle()),
            Tag = DateTime.Now
        };

        _particles.Add(ellipse);
        ParticleCanvas.Children.Add(ellipse);
    }

    private double InitialVelocity()
    {
        return _random.NextDouble() * 100 + 100; // 降低初始速度的范围以适应屏幕动画
    }

    private double RandomAngle()
    {
        return _random.NextDouble() * 180; // 角度从0到180度
    }
}