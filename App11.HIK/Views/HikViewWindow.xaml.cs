using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using App11.HIK.Models;

namespace App11.HIK.Views;

public partial class HikViewWindow : Window
{
    public HikViewWindow()
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
            foreach (var node in robots.Select(robot => new JsNodeWc(robot)))
            {
                Application.Current.Dispatcher.Invoke(() => { HikList.Add(node); });
            }
        });
    }

    public ObservableCollection<JsNodeWc> HikList { get; set; } = new();
}