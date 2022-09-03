using App11.HIK.Concurrent;
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
            foreach (var robot in robots)
            {
                var node = new JsNodeA2(robot);
                A2NodeList.Add(node);
            }
        });
    }

    public MtObservableCollection<JsNodeA2> A2NodeList { get; set; } = new();
}