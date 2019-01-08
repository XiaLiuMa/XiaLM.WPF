using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace XiaLM.WPF.T01.Utility.Windows
{
    /// <summary>
    /// 网格操作助手
    /// </summary>
    public class GridOperator
    {
        /// <summary>
        /// 界面淡入
        /// </summary>
        /// <param name="control"></param>
        /// <param name="second">淡入时间</param>
        public static void Fade_Int_Grid(FrameworkElement control, double second)
        {
            control.Visibility = Visibility.Visible;
            control.IsEnabled = true;

            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,//起始值
                To = 1,//结束值
                Duration = TimeSpan.FromSeconds(second)//动画持续时间
            };
            control.BeginAnimation(UIElement.OpacityProperty, da);//开始动画
        }

        /// <summary>
        /// 界面淡出
        /// </summary>
        /// <param name="window"></param>
        /// <param name="second">淡出时间</param>
        public static void Fade_Out_Grid(FrameworkElement window, double second)
        {
            window.IsEnabled = false;
            Dispatcher mainthread = Dispatcher.CurrentDispatcher;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,//起始值
                To = 0,//结束值
                Duration = TimeSpan.FromSeconds(second)//动画持续时间
            };
            window.BeginAnimation(UIElement.OpacityProperty, da);//开始动画

            ThreadStart start = delegate
            {
                Thread.Sleep((int)(second * 1000));
                mainthread.BeginInvoke((Action)delegate // 异步更新界面
                {
                    window.Visibility = Visibility.Hidden;
                    // 线程结束后的操作
                });
            };
            new Thread(start).Start(); // 启动线程

        }
    }
}
