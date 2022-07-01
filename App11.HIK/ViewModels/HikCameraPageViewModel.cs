using App11.HIK.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace App11.HIK.ViewModels;

public class HikCameraPageViewModel : ObservableObject
{
    public HikCameraPageViewModel(RobotModel robot)
    {
        CurrentRobot = robot;
    }

    public RobotModel CurrentRobot { get; set; }
}