using System;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;

namespace App11.HIK.Helper;

public class FullScreenHelper
{
    /// <summary>
    ///     是否全屏
    /// </summary>
    private bool IsFullScreen { set; get; }

    /// <summary>
    ///     父级容器句柄
    /// </summary>
    private IntPtr OldWndParent { get; } = IntPtr.Zero;

    /// <summary>
    ///     父级容器位置
    /// </summary>
    private WINDOWPLACEMENT OldWndPlacement { set; get; } = new();

    /// <summary>
    ///     需要全屏的视频控件
    /// </summary>
    private System.Windows.Forms.Control VideoControl { get; }

    /// <summary>
    ///     父级容器
    /// </summary>
    private WindowsFormsHost FormsHost { get; }

    /// <summary>
    /// </summary>
    /// <param name="control">需要实现全屏的控件</param>
    /// <param name="formsHost">父级容器</param>
    public FullScreenHelper(System.Windows.Forms.Control control, WindowsFormsHost formsHost)
    {
        VideoControl = control;

        OldWndParent = formsHost.Handle;

        FormsHost = formsHost;
    }

    public struct POINT
    {
        private int x;
        private int y;
    }

    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    /// <summary>
    ///     锁定指定窗口，禁止它更新。同时只能有一个窗口处于锁定状态。锁定指定窗口，禁止它更新。同时只能有一个窗口处于锁定状态
    /// </summary>
    /// <param name="hWndLock"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    private static extern bool LockWindowUpdate(IntPtr hWndLock);

    /// <summary>
    ///     函数来设置弹出式窗口，层叠窗口或子窗口的父窗口。新的窗口与窗口必须属于同一应用程序
    /// </summary>
    /// <param name="hWndChild"></param>
    /// <param name="hWndNewParent"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    /// <summary>
    ///     函数设置指定窗口的显示状态和恢复，最大化，最小化位置。函数功能： 函及原型
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="lpwndpl"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    private static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    /// <summary>
    ///     函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    ///     该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域
    /// </summary>
    /// <returns></returns>
    [DllImport("User32.dll")]
    private static extern IntPtr GetDesktopWindow();

    /// <summary>
    ///     函数名。该函数返回指定窗口的显示状态以及被恢复的、最大化的和最小化的窗口位置
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="lpwndpl"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    /// <summary>
    ///     是用于得到被定义的系统数据或者系统配置信息的一个专有名词
    /// </summary>
    /// <param name="nIndex"></param>
    /// <returns></returns>
    [DllImport("User32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
    public static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int width, int height,
        int flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32")]
    public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern uint ScreenToClient(IntPtr hwnd, ref POINT p);

    public void FullScreen(bool flag)
    {
        IsFullScreen = flag;
        if (!IsFullScreen)
        {
            LockWindowUpdate(VideoControl.Handle);

            SetParent(VideoControl.Handle, OldWndParent);

            //SetWindowPlacement(VideoControl.Handle, ref m_OldWndPlacement);

            FormsHost.Child = VideoControl; /*需要再次把视频控件设置在父容器中，否则会显示在无标题窗口中*/

            SetForegroundWindow(OldWndParent);
            SetForegroundWindow(VideoControl.Handle);

            LockWindowUpdate(IntPtr.Zero);
        }
        else
        {
            //GetWindowPlacement(VideoControl.Parent.Handle, ref m_OldWndPlacement);

            var nScreenWidth = GetSystemMetrics(0);
            var nScreenHeight = GetSystemMetrics(1);

            //m_OldWndParent = GetParent(VideoControl.Parent.Handle);

            SetParent(VideoControl.Handle, GetDesktopWindow());

            var wp1 = new WINDOWPLACEMENT();
            wp1.length = (uint)Marshal.SizeOf(wp1);
            wp1.showCmd = 1;
            wp1.rcNormalPosition.left = 0;
            wp1.rcNormalPosition.top = 0;
            wp1.rcNormalPosition.right = nScreenWidth;
            wp1.rcNormalPosition.bottom = nScreenHeight;
            SetWindowPlacement(VideoControl.Handle, ref wp1);

            SetForegroundWindow(GetDesktopWindow());
            SetForegroundWindow(VideoControl.Handle);
        }
    }

    public struct WINDOWPLACEMENT
    {
        public uint length;
        public uint flags;
        public uint showCmd;
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
    }
}