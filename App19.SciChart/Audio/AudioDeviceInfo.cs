// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioDeviceInfo.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using NAudio.CoreAudioApi;

namespace App19.SciChart00.Audio;

public class AudioDeviceInfo : ObservableObject
{
    public AudioDeviceInfo(MMDevice device)
    {
        _displayName = device.DeviceFriendlyName;
        _id = device.ID;
    }

    private string _displayName;

    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    private string _id;

    public string ID
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    internal void Update(MMDevice device)
    {
        _displayName = device.DeviceFriendlyName;
    }
}