﻿using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using App11.HIK.Data;
using App11.HIK.Helper;
using App11.HIK.HikSdk;
using App11.HIK.Utils;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Timers.Timer;

namespace App11.HIK.Views;

public partial class HowToUseHikWindow
{
    public HowToUseHikWindow()
    {
        InitializeComponent();
        _mControlHandle = VideoControl.Handle;

        InitTimer();
    }

    private readonly IntPtr _mControlHandle; //播放控件句柄
    private Int32 _mUserId = -1; //登录使用
    private Int32 _mStreamHandle = -1; //是否已经开始实时播放
    private Int32 _mStreamPort = -1; //摄像头播放句柄

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        CameraLogout();
    }

    private void CameraLogout()
    {
        //停止录像
        StopRecord();
        //停止预览
        StopPreview();

        //注销登录
        if (_mUserId >= 0)
        {
            CHCNetSDK.NET_DVR_Logout(_mUserId);
            _mUserId = -1;
        }
    }

    private CHCNetSDK.REALDATACALLBACK _realDataCallback;

    private void RealDataCallBack(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser)
    {
        switch (dwDataType)
        {
            case CHCNetSDK.NET_DVR_SYSHEAD: // sys head
                if (dwBufSize > 0)
                {
                    if (_mStreamPort >= 0) return; //同一路码流不需要多次调用开流接口

                    //获取播放句柄 Get the port to play
                    if (!PlayCtrl.PlayM4_GetPort(ref _mStreamPort))
                    {
                        var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                        Log.D("PlayM4_GetPort failed, error code= " + lastErr);
                        break;
                    }

                    //设置流播放模式 Set the stream mode: real-time stream mode
                    if (!PlayCtrl.PlayM4_SetStreamOpenMode(_mStreamPort, PlayCtrl.STREAME_REALTIME))
                    {
                        var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                        Log.D("Set STREAM_REALTIME mode failed, error code= " + lastErr);
                    }

                    //打开码流，送入头数据 Open stream
                    if (!PlayCtrl.PlayM4_OpenStream(_mStreamPort, pBuffer, dwBufSize, 2 * 1024 * 1024))
                    {
                        var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                        Log.D("PlayM4_OpenStream failed, error code= " + lastErr);
                        break;
                    }

                    //设置显示缓冲区个数 Set the display buffer number
                    if (!PlayCtrl.PlayM4_SetDisplayBuf(_mStreamPort, 15))
                    {
                        var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                        Log.D("PlayM4_SetDisplayBuf failed, error code= " + lastErr);
                    }

                    //设置显示模式 Set the display mode
                    if (!PlayCtrl.PlayM4_SetOverlayMode(_mStreamPort, 0, 0 /* COLORREF(0)*/)) //play off screen 
                    {
                        var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                        Log.D("PlayM4_SetOverlayMode failed, error code= " + lastErr);
                    }

                    ////设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data
                    //m_fDisplayFun = new PlayCtrl.DECCBFUN(DecCallbackFUN);
                    //if (!PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, m_fDisplayFun, IntPtr.Zero, 0))
                    //{
                    //}

                    //开始解码 Start to play                       
                    if (!PlayCtrl.PlayM4_Play(_mStreamPort, _mControlHandle))
                    {
                        var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                        Log.D("PlayM4_Play failed, error code= " + lastErr);
                    }
                }

                break;
            case CHCNetSDK.NET_DVR_STREAMDATA: // video stream data
                if (dwBufSize > 0 && _mStreamPort != -1)
                {
                    for (var i = 0; i < 999; i++)
                    {
                        //送入码流数据进行解码 Input the stream data to decode
                        if (!PlayCtrl.PlayM4_InputData(_mStreamPort, pBuffer, dwBufSize))
                        {
                            var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                            Log.D("PlayM4_InputData failed, error code= " + lastErr);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                break;
        }

        //每次收到LIVEVIEW数据时重新开始定时器，当达到规定时间后显示对应的提示图片，表示liveview已经无法获取实时影像
        StartHideTimer();
    }

    private void StartHideTimer()
    {
        try
        {
            _mHideRealPlayTimer.Stop();
            _mHideRealPlayTimer.Close();
            InitTimer();
            _mHideRealPlayTimer.Start();
        }
        catch (Exception ex)
        {
            Log.E("callback error:" + ex.Message);
        }
    }

    private void InitTimer()
    {
        _mHideRealPlayTimer = new Timer();
        _mHideRealPlayTimer.Elapsed += HideRealPlayTimer_Elapsed;

        _mHideRealPlayTimer.Interval = 2000;
        //设置是执行一次（false）还是一直执行(true)；  
        _mHideRealPlayTimer.AutoReset = false;
    }

    private void HideRealPlayTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() => { PanelBack.Visibility = Visibility.Visible; });
    }

    private Timer _mHideRealPlayTimer;

    private void BtnPreview_OnClick(object sender, RoutedEventArgs e)
    {
        if (_mUserId < 0)
        {
            var ret = CameraLogin();
            if (!ret)
            {
                Log.E("设备登录失败");
                MessageBox.Show("无法登录到设备");
                return;
            }
        }

        if (_mStreamHandle < 0)
        {
            var ret = CameraPreview();
            if (!ret)
            {
                Log.E("预览失败");
                return;
            }

            BtnPreview.Content = "停止";
            PanelBack.Visibility = Visibility.Collapsed;
        }
        else
        {
            //停止预览
            StopPreview();
            BtnPreview.Content = "预览";
        }
    }

    private void ButtonShotJpeg_OnClick(object sender, RoutedEventArgs e)
    {
        var ret = TakeShot();
        switch (ret)
        {
            case -1:
                MessageBox.Show("抓图失败，请检查日志");
                break;
            case -2:
            case -3:
                MessageBox.Show("预览时才能抓图");
                break;
        }
    }

    private int TakeShot()
    {
        if (_mUserId < 0) return -3;
        if (_mStreamHandle < 0) return -2;
        //图片保存路径和文件名 the path and file name to save
        var filename = FileUtil.GetShotFilename();

        var lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA
        {
            wPicQuality = 0, //图像质量 Image quality
            wPicSize = 0xff //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档
        };

        //JPEG抓图 Capture a JPEG picture
        if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(_mUserId, 1, ref lpJpegPara, filename))
        {
            var iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            Log.E("NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr);
            return -1;
        }

        Log.I("Successful to capture the JPEG file");
        Log.I("and the saved file is " + filename);
        return 0;
    }

    private bool _mRecording;

    private void ButtonRecord_OnClick(object sender, RoutedEventArgs e)
    {
        if (_mRecording == false)
        {
            var ret = StartRecord();
            switch (ret)
            {
                case 0:
                    BtnRecord.Content = "停止录像";
                    break;
                case -2:
                    MessageBox.Show("预览模式下才能录制！");
                    break;
            }
        }
        else
        {
            var ret = StopRecord();
            if (ret) BtnRecord.Content = "录像";
        }
    }

    private int StartRecord()
    {
        if (_mRecording) return -1;
        if (_mStreamHandle < 0) return -2;
        //录像保存路径和文件名 the path and file name to save
        var filename = FileUtil.GetRecordFilename();
        //强制I帧 Make a I frame
        CHCNetSDK.NET_DVR_MakeKeyFrame(_mUserId, 1);

        //开始录像 Start recording
        if (!CHCNetSDK.NET_DVR_SaveRealData_V30(_mStreamHandle, 2, filename))
        {
            var lastErr = CHCNetSDK.NET_DVR_GetLastError();
            Log.E($"NET_DVR_SaveRealData failed, error code= {lastErr}");
            return -3;
        }

        _mRecording = true;
        return 0;
    }

    private bool StopRecord()
    {
        if (!_mRecording) return false;
        //停止录像 Stop recording
        if (!CHCNetSDK.NET_DVR_StopSaveRealData(_mStreamHandle))
        {
            var lastErr = CHCNetSDK.NET_DVR_GetLastError();
            MessageBox.Show("NET_DVR_StopSaveRealData failed, error code= " + lastErr);
            return false;
        }

        _mRecording = false;
        return true;
    }

    private readonly AppConfig _config = AppConfig.CreateInstance();

    private bool CameraLogin()
    {
        if (_mUserId >= 0) return false;
        var deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
        for (var i = 1; i < 4; i++)
        {
            _mUserId = CHCNetSDK.NET_DVR_Login_V30(_config.HikDemoIp, _config.HikPort, _config.HikUserName,
                _config.HikPassword,
                ref deviceInfo);
            if (_mUserId < 0)
            {
                var lastErr = CHCNetSDK.NET_DVR_GetLastError();
                Log.D("实时影像第" + i + "次登录失败，输出错误号, error code= " + lastErr);
            }
            else
            {
                BtnPreview.Content = "已登录";
                Log.D("实时影像登录成功！");
                return true;
            }
        }

        return false;
    }

    private bool CameraPreview()
    {
        if (_mStreamHandle >= 0) return true;
        var lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO
        {
            lChannel = 1, //预览的设备通道 the device channel number
            dwStreamType = 1, //码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            dwLinkMode = 1, //连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            bBlocked = false, //0- 非阻塞取流，1- 阻塞取流
            dwDisplayBufNum = 1, //播放库显示缓冲区最大帧数
            byProtoType = 1
        };
        var pUser = IntPtr.Zero; //用户数据 user data 

        lpPreviewInfo.hPlayWnd = IntPtr.Zero; //预览窗口 live view window

        _realDataCallback = RealDataCallBack; //预览实时流回调函数 real-time stream callback function 
        _mStreamHandle = CHCNetSDK.NET_DVR_RealPlay_V40(_mUserId, ref lpPreviewInfo, _realDataCallback, pUser);
        if (_mStreamHandle < 0)
        {
            var lastErr = CHCNetSDK.NET_DVR_GetLastError();
            Log.D("NET_DVR_RealPlay_V40 failed, error code= " + lastErr); //预览失败，输出错误号
            return false;
        }
        else
        {
            Log.D("摄像头预览成功！");
            return true;
        }
    }

    private void StopPreview()
    {
        if (_mStreamHandle >= 0)
        {
            if (!CHCNetSDK.NET_DVR_StopRealPlay(_mStreamHandle))
            {
                var lastErr = CHCNetSDK.NET_DVR_GetLastError();
                Log.D("NET_DVR_StopRealPlay failed, error code= " + lastErr);
            }

            _mStreamHandle = -1;
        }

        if (_mStreamPort >= 0)
        {
            if (!PlayCtrl.PlayM4_Stop(_mStreamPort))
            {
                var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                Log.D("PlayM4_Stop failed, error code= " + lastErr);
            }

            if (!PlayCtrl.PlayM4_CloseStream(_mStreamPort))
            {
                var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                Log.D("PlayM4_CloseStream failed, error code= " + lastErr);
            }

            if (!PlayCtrl.PlayM4_FreePort(_mStreamPort))
            {
                var lastErr = PlayCtrl.PlayM4_GetLastError(_mStreamPort);
                Log.D("PlayM4_FreePort failed, error code= " + lastErr);
            }

            _mStreamPort = -1;
        }

        Log.D("关闭摄像头预览");
    }

    private FullScreenHelper _fullScreenHelper;

    private void VideoControl_OnDoubleClick(object sender, EventArgs e)
    {
        _fullScreenHelper = new FullScreenHelper(VideoControl, FormsHost);
        _fullScreenHelper.FullScreen(true);
    }

    private void VideoControl_OnKeyUp(object sender, KeyEventArgs e)
    {
        // ESC 退出全屏
        if (e.KeyCode == Keys.Escape && _fullScreenHelper != null)
        {
            _fullScreenHelper.FullScreen(false);
            _fullScreenHelper = null;
        }
    }
}