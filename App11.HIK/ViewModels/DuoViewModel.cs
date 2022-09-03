using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using App11.HIK.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App11.HIK.ViewModels;

public class DuoViewModel : ObservableObject
{
    public DuoViewModel()
    {
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
                foreach (var node in robots.Select(robot => new JsNodeA2(robot)))
                {
                    A2NodeList.Add(node);
                }
            });
        });
    }

    public ObservableCollection<JsNodeA2> A2NodeList { get; set; } = new();
}