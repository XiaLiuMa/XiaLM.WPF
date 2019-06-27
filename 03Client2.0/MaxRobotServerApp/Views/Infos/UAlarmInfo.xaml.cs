using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaxRobotServerApp.Views.Infos
{
    /// <summary>
    /// UAlarmInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UAlarmInfo : UserControl
    {
        public UAlarmInfo(UAlarmInfoViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        /// <summary>
        /// 展示图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowImg_Click(object sender, RoutedEventArgs e)
        {
            string url = (this.DataContext as UAlarmInfoViewModel).ImgUrl;
            ShowImgFlyout flyout = new ShowImgFlyout(url);
            flyout.ShowDialog();
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //根据报警类型跳转
        }
    }
}
