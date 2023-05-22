using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using App11.HIK.Models;

namespace App11.HIK.Views;

public partial class MultiHikWindow : Window
{
    public MultiHikWindow()
    {
        InitializeComponent();
        DoDummy();
    }

    private void DoDummy()
    {
        var task = JsNode.CreateDummy();
        task.ContinueWith(_ =>
        {
            var robots = task.Result;
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var node in robots.Select(robot => new JsNodeWc(robot))) NodeWcList.Add(node);
            });
        });
    }

    public ObservableCollection<JsNodeWc> NodeWcList { get; set; } = new();
}