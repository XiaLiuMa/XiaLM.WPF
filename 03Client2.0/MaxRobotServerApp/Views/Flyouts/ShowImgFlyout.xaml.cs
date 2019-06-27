using System;
using System.Windows;
using System.Windows.Media.Imaging;


namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// ShowImgFlyout .xaml 的交互逻辑
    /// </summary>
    public partial class ShowImgFlyout : Window
    {
        /// <summary>
        /// 图片展示
        /// </summary>
        /// <param name="url">图片地址</param>
        public ShowImgFlyout(string url)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            var _bitmapImg = new BitmapImage();
            _bitmapImg.BeginInit();
            _bitmapImg.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            _bitmapImg.EndInit();
            this.showImg.Source = _bitmapImg;
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
    }
}
