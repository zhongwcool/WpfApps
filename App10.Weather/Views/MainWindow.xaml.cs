﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using App10.Weather.Helper;
using App10.Weather.Models;
using App10.Weather.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using InteractiveDataDisplay.WPF;
using ModernWpf;

namespace App10.Weather.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();

        WeakReferenceMessenger.Default.Register<Message>(this, OnReceive);

        var x = Enumerable.Range(0, 1001).Select(i => i / 10.0).ToArray();
        var y = x.Select(v => Math.Abs(v) < 1e-10 ? 1 : Math.Sin(v) / v).ToArray();
        linegraph.Plot(x, y); // x and y are IEnumerable<double>
    }

    private void OnReceive(object recipient, Message message)
    {
        switch (message.Key)
        {
            case 101:
                var hm = (HourlyModel)message.Obj;
            {
                Dispatcher.Invoke(() => { GetDataHourly(hm); });
            }
                break;

            case 102:
                var dm = (DailyModel)message.Obj;
            {
                Dispatcher.Invoke(() => { DrawDataDaily(dm); });
            }
                break;
        }
    }

    private void GetDataHourly(HourlyModel hm)
    {
        if (null == hm) return;
        var count = hm.Hourly.Count;
        var x = new double[count];
        var y = new double[count];
        for (var i = 0; i < count; i++)
        {
            x[i] = i;
            y[i] = double.Parse(hm.Hourly[i].Temperature);
        }

        lines.Children.Clear();
        var line1 = new LineGraph();
        lines.Children.Add(line1);
        line1.Stroke = new SolidColorBrush(Colors.Red);
        line1.Description = "最高温";
        line1.StrokeThickness = 3;
        line1.Plot(x, y);
    }

    private void DrawDataDaily(DailyModel dm)
    {
        if (null == dm) return;
        var count = dm.Daily.Count;
        var x = new double[count];
        var y = new double[count];
        var z = new double[count];
        for (var i = 0; i < count; i++)
        {
            x[i] = i;
            y[i] = double.Parse(dm.Daily[i].High);
            z[i] = double.Parse(dm.Daily[i].Low);
        }

        lines.Children.Clear();

        var line1 = new LineGraph();
        lines.Children.Add(line1);
        line1.Stroke = new SolidColorBrush(Colors.Red);
        line1.Description = "最高温";
        line1.StrokeThickness = 3;
        line1.Plot(x, y);

        var line2 = new LineGraph();
        lines.Children.Add(line2);
        line2.Stroke = new SolidColorBrush(Colors.Blue);
        line2.Description = "最低温";
        line2.StrokeThickness = 3;
        line2.Plot(x, z);
    }

    private void ButtonTheme_OnClick(object sender, RoutedEventArgs e)
    {
        DispatcherHelper.RunOnMainThread(() =>
        {
            if (ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
        });
    }
}