using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace WpfApp7;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private delegate void DoPrintMethod(PrintDialog dialog, DocumentPaginator paginator);

    private delegate void EnableButtonMethod();

    private Timer _mTimerToEnableButton;

    private static void DoPrint(PrintDialog dialog, DocumentPaginator paginator)
    {
        dialog.PrintDocument(paginator, "Order Document");
    }

    private void EnableButton()
    {
        BtnPrintDirect.IsEnabled = true;
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnPrintPreview_Click(object sender, RoutedEventArgs e)
    {
        var previewWnd =
            new PrintPreviewWindow("OrderDocument.xaml", GlobalData.m_orderExample, new OrderDocumentRenderer())
            {
                Owner = this,
                ShowInTaskbar = false
            };
        previewWnd.ShowDialog();
    }

    private void btnPrintDlg_Click(object sender, RoutedEventArgs e)
    {
        var pdl = new PrintDialog();
        if (pdl.ShowDialog() != true) return;
        var doc = PrintPreviewWindow.LoadDocumentAndRender("OrderDocument.xaml", GlobalData.m_orderExample,
            new OrderDocumentRenderer());
        Dispatcher.BeginInvoke(new DoPrintMethod(DoPrint), DispatcherPriority.ApplicationIdle, pdl,
            ((IDocumentPaginatorSource)doc).DocumentPaginator);
    }

    private void btnPrintDirect_Click(object sender, RoutedEventArgs e)
    {
        BtnPrintDirect.IsEnabled = false;
        var dialog = new PrintDialog();
        var doc = PrintPreviewWindow.LoadDocumentAndRender("OrderDocument.xaml", GlobalData.m_orderExample,
            new OrderDocumentRenderer());
        Dispatcher.BeginInvoke(new DoPrintMethod(DoPrint), DispatcherPriority.ApplicationIdle, dialog,
            ((IDocumentPaginatorSource)doc).DocumentPaginator);
        _mTimerToEnableButton = new Timer(TestTimerCallback, null, 3000, Timeout.Infinite);
    }

    private void TestTimerCallback(object state)
    {
        _mTimerToEnableButton.Dispose();
        Dispatcher.BeginInvoke(new EnableButtonMethod(EnableButton));
    }
}