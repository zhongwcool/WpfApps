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
                A2NodeList.Add(node);
            }

            HikView0.SetNode(A2NodeList[0]);
            HikView1.SetNode(A2NodeList[1]);
            HikView2.SetNode(A2NodeList[2]);
            HikView3.SetNode(A2NodeList[3]);
        });
    }

    private ObservableCollection<JsNodeWc> A2NodeList { get; set; } = new();
}