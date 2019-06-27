using EasyPlayerSdk;

using MaxRobotServerApp.Dals;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// RobotInstructionFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class RobotInstructionFlyout : Window
    {
        private RobotInfo robot;
        private bool isInit = false;
        private bool isTCP = true;
        private bool isHardEncode = false;
        private RtspPlayerSdk.MediaSourceCallBack callBack = null;
        private RtspPlayerSdk.RENDER_FORMAT RENDER_FORMAT = RtspPlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI;

        public RobotInstructionFlyout(RobotInfo obj)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            robot = obj;
            isInit = (RtspPlayerSdk.EasyPlayer_Init() >= 0) ? true : false;
            callBack = new RtspPlayerSdk.MediaSourceCallBack(MediaCallback);
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            if (channelID1a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID1a);
            if (channelID1b != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID1b);
            if (channelID2a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID2a);
            if (channelID3a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID3a);
            this.Close();
        }

        #region 视频监控
        private int channelID1a = -1;
        private int channelID1b = -1;
        private int channelID2a = -1;
        private int channelID3a = -1;

        /// <summary>
        /// Table切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.tbControl.SelectedIndex)
            {
                case 0: //分屏视频
                    {
                        if (channelID2a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID2a);
                        if (channelID2a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID2a);

                        //string RTSPStreamURI = $"rtsp://{robot.IPCameraIp}:{robot.IPCameraPort}/user=admin&password=123456&channel=1&stream=0.sdp?";
                        string RTSPStreamURI1 = "rtsp://184.72.239.149/vod/mp4://BigBuckBunny_175k.mov";
                        IntPtr hwnd1 = this.tbItem11Panle.Handle;
                        channelID1a = RtspPlayerSdk.EasyPlayer_OpenStream(RTSPStreamURI1, hwnd1, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                        if (channelID1a > 0) RtspPlayerSdk.EasyPlayer_SetFrameCache(channelID1a, 3);

                        //string RTSPStreamURI = $"rtsp://admin:a12345678@{robot.InfraredCameraIP}:{robot.InfraredCameraPort}/h264/ch1/main/av_stream";
                        string RTSPStreamURI2 = "rtsp://184.72.239.149/vod/mp4://BigBuckBunny_175k.mov";
                        IntPtr hwnd2 = this.tbItem12Panle.Handle;
                        channelID1b = RtspPlayerSdk.EasyPlayer_OpenStream(RTSPStreamURI2, hwnd2, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                        if (channelID1b > 0) RtspPlayerSdk.EasyPlayer_SetFrameCache(channelID1b, 3);
                    }
                    break;
                case 1: //实时视频
                    {
                        if (channelID1a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID1a); //
                        if (channelID1b != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID1b); //
                        if (channelID3a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID3a); //

                        //string RTSPStreamURI = $"rtsp://{robot.IPCameraIp}:{robot.IPCameraPort}/user=admin&password=123456&channel=1&stream=0.sdp?";
                        string RTSPStreamURI = "rtsp://184.72.239.149/vod/mp4://BigBuckBunny_175k.mov";
                        IntPtr hwnd = this.tbItem2Panle.Handle;
                        channelID2a = RtspPlayerSdk.EasyPlayer_OpenStream(RTSPStreamURI, hwnd, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                        if (channelID2a > 0) RtspPlayerSdk.EasyPlayer_SetFrameCache(channelID2a, 3);
                    }
                    break;
                case 2: //红外视频
                    {
                        if (channelID1a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID1a); //
                        if (channelID1b != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID1b); //
                        if (channelID2a != -1) RtspPlayerSdk.EasyPlayer_CloseStream(channelID2a); //

                        //string RTSPStreamURI = $"rtsp://admin:a12345678@{robot.InfraredCameraIP}:{robot.InfraredCameraPort}/h264/ch1/main/av_stream";
                        string RTSPStreamURI = "rtsp://184.72.239.149/vod/mp4://BigBuckBunny_175k.mov";
                        IntPtr hwnd = this.tbItem3Panle.Handle;
                        channelID3a = RtspPlayerSdk.EasyPlayer_OpenStream(RTSPStreamURI, hwnd, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                        if (channelID3a > 0) RtspPlayerSdk.EasyPlayer_SetFrameCache(channelID3a, 3);
                    }
                    break;
            }
        }

        /// <summary>
        /// 数据流回调
        /// </summary>
        /// <param name="_channelId">The _channel identifier.</param>
        /// <param name="_channelPtr">The _channel PTR.</param>
        /// <param name="_frameType">Type of the _frame.</param>
        /// <param name="pBuf">The p buf.</param>
        /// <param name="_frameInfo">The _frame information.</param>
        /// <returns>System.Int32.</returns>
        private int MediaCallback(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref RtspPlayerSdk.EASY_FRAME_INFO _frameInfo)
        {
            return 0;
        }
        #endregion

        /// <summary>
        /// 抬头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaiTou_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C103");
        }

        /// <summary>
        /// 低头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiTou_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C104");
        }

        /// <summary>
        /// 向左看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZuoKan_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C105");
        }

        /// <summary>
        /// 向右看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YouKan_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C106");
        }

        /// <summary>
        /// 回原位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HuiYuanWei_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C107");
        }

        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QianJin_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C118");
        }

        /// <summary>
        /// 左转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZuoZhuan_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C119");
        }

        /// <summary>
        /// 右转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YouZhuan_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C120");
        }

        /// <summary>
        /// 向后转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HouZhuan_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C121");
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TingZhi_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            dal.SendCmd(robot.Tag, "C122");
        }
    }
}
