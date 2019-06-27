using System.Windows;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// RemindFlyout .xaml 的交互逻辑
    /// </summary>
    public partial class RemindFlyout : Window
    {
        /// <summary>
        /// 是否点击的确定
        /// </summary>
        public bool IsConfirm { get; set; }

        /// <summary>
        /// 延时关闭时间
        /// </summary>
        /// <param name="txt">报警信息</param>
        /// <param name="delay"></param>
        public RemindFlyout(string txt)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.waringTxt.Text = txt;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            IsConfirm = true;
            this.Close();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsConfirm = false;
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
