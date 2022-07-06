using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using App06.Dll.Models;

namespace App06.Dll.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += Window_Loaded;

        var dll = new Thread(() =>
        {
            var data = hello();
            Dispatcher.Invoke(() => { MyBlock.AppendText($"结果：{data}\n"); });
        });
        dll.Start();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var list = new List<object> { "a", 1, "b", 2, new Foo() { Name = "Brian" } };
        treeView1.DataContext = list;
    }

    [DllImport("libs/DemoDll.dll", EntryPoint = "hello", CallingConvention = CallingConvention.Cdecl)]
    private static extern int hello();

    [DllImport("libs/DemoDll.dll", CharSet = CharSet.Unicode, EntryPoint = "SayHello",
        CallingConvention = CallingConvention.Cdecl)]
    private static extern void SayHello(byte[] str, int length);

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        const int STRING_MAX_LENGTH = 512;
        var msg = new byte[STRING_MAX_LENGTH];

        SayHello(msg, STRING_MAX_LENGTH);
        MyBlock.AppendText($"remote says: {Encoding.ASCII.GetString(msg)}\n");
    }
}