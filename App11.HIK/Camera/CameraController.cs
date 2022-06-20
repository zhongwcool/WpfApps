using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using App11.HIK.Data;
using App11.HIK.Utils;

namespace App11.HIK.Camera;

public class CameraController
{
    private CameraController()
    {
        playFormsHost.Child = RealPlayWnd;
        playFormsHost.Visibility = Visibility.Visible;
        RealPlayWnd.Image = m_Icon;
        RealPlayWnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
        m_ptrRealHandle = RealPlayWnd.Handle;
        InitTimer();
    }

    public static CameraController Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CameraController();
                    }
                }
            }

            return instance;
        }
    }

    public void Display(Panel panel)
    {
        var a = VisualTreeHelper.GetParent(playFormsHost);
        if (a == null)
        {
            panel.Children.Add(playFormsHost);
        }
        else
        {
            Panel oldPanel = a as Panel;
            oldPanel.Children.Clear();
            panel.Children.Add(playFormsHost);
        }
    }

    public bool CameraInit()
    {
        bool ret = false;

        if (CHCNetSDK.NET_DVR_Init())
        {
            CHCNetSDK.NET_DVR_SetLogToFile(3, ".\\SdkLog\\", true);
            ret = true;
        }
        else
        {
            Log.D("摄像头初始化失败！");
        }

        return ret;
    }

    private readonly AppConfig _config = AppConfig.CreateInstance();

    public bool CameraLogin()
    {
        bool ret = false;
        if (m_lUserID < 0)
        {
            //TODO:需要改为自己的网络摄像头配置
            string DVRIPAddress = _config.IPAddress; //设备IP地址或者域名
            Int16 DVRPortNumber = _config.Port; // 设备服务端口号
            string DVRUserName = _config.UserName; //设备登录用户名
            string DVRPassword = _config.Password; //设备登录密码

            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            for (int i = 1; i < 4; i++)
            {
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword,
                    ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    string str = "实时影像第" + i + "次登录失败，输出错误号, error code= " + iLastErr;
                    Log.D(str);
                }
                else
                {
                    CameraPreview();
                    string str = "实时影像登录成功！ ";
                    Log.D(str);
                    ret = true;
                    break;
                }
            }
        }

        return ret;
    }
    
    public void CameraLogout()
    {
        //停止预览
        ClosePreview();

        //注销登录
        if (m_lUserID >= 0)
        {
            CHCNetSDK.NET_DVR_Logout(m_lUserID);
            m_lUserID = -1;
        }

        CHCNetSDK.NET_DVR_Cleanup();
    }

    private void CameraPreview()
    {
        if (m_lRealHandle < 0)
        {
            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.lChannel = 1; //预览的设备通道 the device channel number
            lpPreviewInfo.dwStreamType = 1; //码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            lpPreviewInfo.dwLinkMode = 1; //连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = false; //0- 非阻塞取流，1- 阻塞取流
            lpPreviewInfo.dwDisplayBufNum = 1; //播放库显示缓冲区最大帧数
            lpPreviewInfo.byProtoType = 1;
            IntPtr pUser = IntPtr.Zero; //用户数据 user data 

            lpPreviewInfo.hPlayWnd = IntPtr.Zero; //预览窗口 live view window

            RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack); //预览实时流回调函数 real-time stream callback function 
            m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);
            if (m_lRealHandle < 0)
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
                //MessageBox.Show(str);
                Log.D(str);
                return;
            }
            else
            {
                Log.D("摄像头预览成功！");
            }
        }
    }

    private void ClosePreview()
    {
        if (m_lRealHandle > 0)
        {
            if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
            {
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                string str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                Log.D(str);
                return;
            }
        }

        if (m_lPort >= 0)
        {
            if (!PlayCtrl.PlayM4_Stop(m_lPort))
            {
                uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                string str = "PlayM4_Stop failed, error code= " + iLastErr;
                Log.D(str);
            }

            if (!PlayCtrl.PlayM4_CloseStream(m_lPort))
            {
                uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                string str = "PlayM4_CloseStream failed, error code= " + iLastErr;
                Log.D(str);
            }

            if (!PlayCtrl.PlayM4_FreePort(m_lPort))
            {
                uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                string str = "PlayM4_FreePort failed, error code= " + iLastErr;
                Log.D(str);
            }

            m_lPort = -1;
        }

        m_lRealHandle = -1;
    }
    
    private void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
    {
        switch (dwDataType)
        {
            case CHCNetSDK.NET_DVR_SYSHEAD: // sys head
                if (dwBufSize > 0)
                {
                    if (m_lPort >= 0)
                    {
                        return; //同一路码流不需要多次调用开流接口
                    }

                    //获取播放句柄 Get the port to play
                    if (!PlayCtrl.PlayM4_GetPort(ref m_lPort))
                    {
                        uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        string str = "PlayM4_GetPort failed, error code= " + iLastErr;
                        Log.D(str);
                        break;
                    }

                    //设置流播放模式 Set the stream mode: real-time stream mode
                    if (!PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, PlayCtrl.STREAME_REALTIME))
                    {
                        uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        string str = "Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                        Log.D(str);
                    }

                    //打开码流，送入头数据 Open stream
                    if (!PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 2 * 1024 * 1024))
                    {
                        uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        string str = "PlayM4_OpenStream failed, error code= " + iLastErr;
                        Log.D(str);
                        break;
                    }
                    
                    //设置显示缓冲区个数 Set the display buffer number
                    if (!PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                    {
                        uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        string str = "PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                        Log.D(str);
                    }

                    //设置显示模式 Set the display mode
                    if (!PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0 /* COLORREF(0)*/)) //play off screen 
                    {
                        uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        string str = "PlayM4_SetOverlayMode failed, error code= " + iLastErr;
                        Log.D(str);
                    }

                    ////设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data
                    //m_fDisplayFun = new PlayCtrl.DECCBFUN(DecCallbackFUN);
                    //if (!PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, m_fDisplayFun, IntPtr.Zero, 0))
                    //{
                    //}

                    //开始解码 Start to play                       
                    if (!PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
                    {
                        uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        string str = "PlayM4_Play failed, error code= " + iLastErr;
                        Log.D(str);
                        break;
                    }
                }

                break;
            case CHCNetSDK.NET_DVR_STREAMDATA: // video stream data
                if (dwBufSize > 0 && m_lPort != -1)
                {
                    for (int i = 0; i < 999; i++)
                    {
                        //送入码流数据进行解码 Input the stream data to decode
                        if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                        {
                            uint iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            string str = "PlayM4_InputData failed, error code= " + iLastErr;
                            Log.D(str);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                break;
            default:
                break;
        }

        //每次收到LIVEVIEW数据时重新开始定时器，当达到规定时间后显示对应的提示图片，表示liveview已经无法获取实时影像
        StartHideTimer();
    }

    private void DecCallbackFUN(int nPort, IntPtr pBuf, int nSize, ref PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1,
        int nReserved2)
    {
    }

    void StartHideTimer()
    {
        try
        {
            m_HideRealPlayTimer.Stop();
            m_HideRealPlayTimer.Close();
            InitTimer();
            m_HideRealPlayTimer.Start();
        }
        catch (Exception ex)
        {
            Log.E(" callback error:" + ex.Message);
        }
    }
    
    void InitTimer()
    {
        m_HideRealPlayTimer = new System.Timers.Timer();
        m_HideRealPlayTimer.Elapsed += HideRealPlayTimer_Elapsed;

        m_HideRealPlayTimer.Interval = 2000;
        //设置是执行一次（false）还是一直执行(true)；  
        m_HideRealPlayTimer.AutoReset = false;
    }

    private void HideRealPlayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        RealPlayWnd.Image = m_Icon;
    }

    private static Bitmap BitmapImage2Bitmap(string uri)
    {
        BitmapImage bitmapImage = new BitmapImage(new Uri(uri));

        using (MemoryStream outStream = new MemoryStream())
        {
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapImage));
            enc.Save(outStream);
            Bitmap bitmap = new Bitmap(outStream);

            return new Bitmap(bitmap);
        }
    }

    private Int32 m_lUserID = -1; //登录使用
    private Int32 m_lRealHandle = -1; //是否已经开始实时播放
    private Int32 m_lPort = -1; //摄像头播放句柄
    private System.Windows.Forms.PictureBox RealPlayWnd = new();
    private System.Windows.Forms.Integration.WindowsFormsHost playFormsHost = new();

    private IntPtr m_ptrRealHandle; //播放控件句柄
    private PlayCtrl.DECCBFUN m_fDisplayFun = null;
    private System.Timers.Timer m_HideRealPlayTimer = null;
    private CHCNetSDK.REALDATACALLBACK RealData = null;
    private Bitmap m_Icon = BitmapImage2Bitmap($"pack://application:,,,/Resources/ForbidenSign.png");
    
    private static volatile CameraController instance;
    private static readonly object syncRoot = new object();
}