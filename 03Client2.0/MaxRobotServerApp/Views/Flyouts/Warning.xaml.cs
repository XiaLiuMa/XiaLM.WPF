using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// Warning .xaml 的交互逻辑
    /// </summary>
    public partial class Warning : Window
    {
        /// <summary>
        /// 延时关闭时间
        /// </summary>
        /// <param name="txt">报警信息</param>
        /// <param name="delay"></param>
        public Warning(string txt,int delay = 3*1000)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            if (!string.IsNullOrEmpty(txt))
            {
                this.waringTxt.Text = txt;
            }
            DelayShutDown(delay);
        }

        /// <summary>
        /// 延时关闭
        /// </summary>
        /// <param name="delay"></param>
        private void DelayShutDown(int delay)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay);
                this.Dispatcher?.Invoke(new Action(()=> 
                {
                    this?.Close();
                }));
            });
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 配置允许拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 获取鼠标相对标题栏位置 
            Point position = e.GetPosition(this);

            // 如果鼠标位置在标题栏内，允许拖动 
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (position.X >= 0 && position.X < this.ActualWidth && position.Y >= 0 && position.Y < this.ActualHeight)
                {
                    this.DragMove();
                }
            }
        }
    }
}
