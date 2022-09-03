using App11.HIK.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App11.HIK.ViewModels;

public class HikPageViewModel : ObservableObject
{
    public HikPageViewModel(JsNode robot)
    {
        CurrentRobot = robot;
    }

    public JsNode CurrentRobot { get; set; }
}