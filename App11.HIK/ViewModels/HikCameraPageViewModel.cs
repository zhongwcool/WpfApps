using App11.HIK.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace App11.HIK.ViewModels;

public class HikCameraPageViewModel : ObservableObject
{
    private HikCameraPageViewModel(RobotModel robot)
    {
        CurrentRobot = robot;
    }

    private static HikCameraPageViewModel _instance;

    public static HikCameraPageViewModel CreateInstance(RobotModel robot)
    {
        _instance ??= new HikCameraPageViewModel(robot);
        return _instance;
    }

    public RobotModel CurrentRobot { get; set; }
}