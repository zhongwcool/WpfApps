using App11.HIK.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App11.HIK.ViewModels;

public class HikCameraPageViewModel : ObservableObject
{
    public HikCameraPageViewModel(JsNode robot)
    {
        CurrentRobot = robot;
    }

    public JsNode CurrentRobot { get; set; }
}