using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;

namespace App07.Print.Views;

public partial class PrintPreviewWindow : Window
{
    private delegate void LoadXpsMethod();

    private readonly object _mData;
    private readonly FlowDocument _mDoc;

    public static FlowDocument LoadDocumentAndRender(string strTmplName, object data, IDocumentRenderer renderer)
    {
        var doc = (FlowDocument)Application.LoadComponent(new Uri(strTmplName, UriKind.RelativeOrAbsolute));
        doc.PagePadding = new Thickness(50);
        doc.DataContext = data;
        renderer?.Render(doc, data);
        return doc;
    }

    public PrintPreviewWindow(string strTmplName, object data, IDocumentRenderer renderer = null)
    {
        InitializeComponent();
        _mData = data;
        _mDoc = LoadDocumentAndRender(strTmplName, data, renderer);
        Dispatcher.BeginInvoke(new LoadXpsMethod(LoadXps), DispatcherPriority.ApplicationIdle);
    }

    private void LoadXps()
    {
        //构造一个基于内存的xps document
        var ms = new MemoryStream();
        var package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
        var documentUri = new Uri("pack://InMemoryDocument.xps");
        PackageStore.RemovePackage(documentUri);
        PackageStore.AddPackage(documentUri, package);
        var xpsDocument = new XpsDocument(package, CompressionOption.Fast, documentUri.AbsoluteUri);

        //将flow document写入基于内存的xps document中去
        var writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
        writer.Write(((IDocumentPaginatorSource)_mDoc).DocumentPaginator);

        //获取这个基于内存的xps document的fixed document
        DocViewer.Document = xpsDocument.GetFixedDocumentSequence();

        //关闭基于内存的xps document
        xpsDocument.Close();
    }
}