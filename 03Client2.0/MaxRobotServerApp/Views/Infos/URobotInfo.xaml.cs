using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk.Model;
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
    /// URobotInfo.xaml 的交互逻辑
    /// </summary>
    public partial class URobotInfo : UserControl
    {
        /// <summary>
        /// 移除机器人事件
        /// </summary>
        public event Action<string> RemoveRobotByTag = (p)=> { };

        public URobotInfo(URobotInfoViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        /// <summary>
        /// 编辑机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RobotEdit_Click(object sender, RoutedEventArgs e)
        {
            string tag = (this.DataContext as URobotInfoViewModel).Tag;
            string name = (this.DataContext as URobotInfoViewModel).RobotName;
            RobotInfoFlyout flyout = new RobotInfoFlyout(tag, name);
            flyout.ConfirmSuccess += Flyout_ConfirmSuccess;
            flyout.ShowDialog();
        }

        /// <summary>
        /// 修改提交事件
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="name"></param>
        private void Flyout_ConfirmSuccess(string tag,string name)
        {
            RobotMangerDal dal = new RobotMangerDal();
            RobotInfo info = dal.GetRobotInfo(tag);
            info.Name = name;
            bool flag = dal.UpdataRobotInfo(info);
            if(flag)
            {
                (this.DataContext as URobotInfoViewModel).RobotName = info.Name;
            }
        }

        /// <summary>
        /// 关闭机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RobotClose_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在移除机器人");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                RobotMangerDal dal = new RobotMangerDal();
                bool flag = dal.DeleteRobotInfo((this.DataContext as URobotInfoViewModel).Tag);
                if (flag)
                {
                    RemoveRobotByTag((this.DataContext as URobotInfoViewModel).Tag);
                }
            }
        }

        /// <summary>
        /// 机器人故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorState_Click(object sender, RoutedEventArgs e)
        {
            RobotErrorStateFlyout flyout = new RobotErrorStateFlyout((this.DataContext as URobotInfoViewModel).ErrorList);
            flyout.ShowDialog();
        }
        
        /// <summary>
        /// 指令视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Instruction_Click(object sender, RoutedEventArgs e)
        {
            RobotMangerDal dal = new RobotMangerDal();
            RobotInfo info = dal.GetRobotInfo((this.DataContext as URobotInfoViewModel).Tag);
            RobotInstructionFlyout flyout = new RobotInstructionFlyout(info);
            flyout.ShowDialog();
        }
    }
}
