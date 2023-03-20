using System;
using System.Collections.ObjectModel;
using System.Windows;
using App11.HIK.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App11.HIK.ViewModels;

public class InPageViewModel : ObservableObject
{
    public InPageViewModel()
    {
        DeleteCommand = new RelayCommand<JsNode>(DoDelete);

        // Create dummy data
        DoDummy();
    }

    private void DoDummy()
    {
        var task = JsNode.CreateDummy();
        task.ContinueWith(_ =>
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                var robots = task.Result;
                foreach (var robot in robots)
                {
                    RobotList.Add(robot);
                }
            }));
        });
    }

    public ObservableCollection<JsNode> RobotList { get; set; } = new();

    public IRelayCommand<JsNode> DeleteCommand { get; set; }

    private void DoDelete(JsNode selectedNode)
    {
        RobotList.Remove(selectedNode);
    }
}