using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Toolkit.Mvvm.Messaging;
using WpfApp4.Config;
using WpfApp4.Models;
using WpfApp4.ViewModel;

namespace WpfApp4;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainViewModel.CreateInstance();

        if (LanguageConfig.Instance.LanguageCurrent == "0")
        {
            rb_cn.IsChecked = true;
            rb_en.IsChecked = false;
            // pack://application:,,,/BroadCommon;component/style/lang/zh-cn.xaml" 跨包资源格式
            LoadLanguageFile("pack://application:,,,/Lang/zh-cn.xaml");
        }
        else
        {
            rb_cn.IsChecked = false;
            rb_en.IsChecked = true;
            LoadLanguageFile("pack://application:,,,/Lang/en-us.xaml");
        }

        WeakReferenceMessenger.Default.Register<Message>(this, OnReceive);
    }

    private void OnReceive(object recipient, Message message)
    {
        switch (message.Num)
        {
            case 123:
            {
                Dispatcher.Invoke(() => { Info.Text = message.Str; });
            }
                break;
        }
    }

    private static void LoadLanguageFile(string languageFileName)
    {
        Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary
        {
            Source = new Uri(languageFileName, UriKind.RelativeOrAbsolute)
        };
    }

    private void RadioButton_Click(object sender, RoutedEventArgs e)
    {
        var rbtn = sender as RadioButton;
        if (rbtn.Tag.ToString().Trim() == "0")
        {
            LoadLanguageFile("pack://application:,,,/Lang/zh-cn.xaml");
            LanguageConfig.Instance.LanguageCurrent = "0";
        }
        else
        {
            LoadLanguageFile("pack://application:,,,/Lang/en-us.xaml");
            LanguageConfig.Instance.LanguageCurrent = "1";
        }
    }
}