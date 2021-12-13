using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace WpfApp6;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var dll = new Thread(() =>
        {
            var data = hello();
            Dispatcher.Invoke(() => { MyBlock.AppendText($"结果：{data}\n"); });
        });
        dll.Start();
    }

    [DllImport("libs/libDemoDll2.dll", EntryPoint = "hello", CallingConvention = CallingConvention.Cdecl)]
    private static extern int hello();

    [DllImport("libs/libDemoDll2.dll", CharSet = CharSet.Unicode, EntryPoint = "SayHello",
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