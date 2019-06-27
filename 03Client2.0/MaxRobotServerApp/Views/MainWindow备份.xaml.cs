using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MaxRobotServerApp.Comm;
using MaxRobotServerApp.Views.Flyouts;
using Maxvision.Robot.Sdk.Model;

namespace MaxRobotServerApp.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow备份 : MetroWindow
    {
        public MainWindow备份()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 一级菜单展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExpanderEvent(object sender, MouseButtonEventArgs e)
        {
            Expander sd = sender as Expander;
            if (sd != null)
            {
                //关闭其它的一级菜单展开
                foreach (var item in this.menu.Children)
                {
                    Expander ed = item as Expander;
                    if (ed == null) continue;
                    if (!ed.Equals(sd))
                    {
                        ed.IsExpanded = false;
                    }
                }
            }
        }

        /// <summary>
        /// 菜单被选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSelectEvent(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in this.menu.Children)
            {
                Expander ed = item as Expander;
                if (ed == null) continue;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).BottomFlyout.IsOpen = !((MainWindow)Application.Current.MainWindow).BottomFlyout.IsOpen;
        }

        /// <summary>
        /// 主题设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Robotmanager_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Robotmanager.xaml");
        }

        /// <summary>
        /// 语义库管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Semanticlibrarymanager_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Semanticlibrarymanager.xaml");
        }

        /// <summary>
        /// 语义管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Semanticmanager_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Semanticmanager.xaml");
        }

        /// <summary>
        /// 宣讲资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Preachresources_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Preachresources.xaml");
        }

        /// <summary>
        /// App资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appsources_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Appsources.xaml");
        }

        /// <summary>
        /// 下载管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadManage_Click(object sender, RoutedEventArgs e)
        {
            ViewManager.GetInstance().downloadFlyout.Show();
        }

        /// <summary>
        /// 用户配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Userconfig_Click(object sender, RoutedEventArgs e)
        {
            switch (UserManager.GetInstance().User.UserType)
            {
                case UserType.Admin:
                    {
                        JumpPage(@"/Views/Bodys/Userconfig.xaml");
                    }
                    break;
                case UserType.User:
                    {
                        new Warning("对不起，您没有权限！").Show();    //无权限
                    }
                    break;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Changepwd_Click(object sender, RoutedEventArgs e)
        {
            Changepwd changepwd = new Changepwd(UserManager.GetInstance().User.Name);
            changepwd.Show();
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserManager.GetInstance().RclientLoginOut();
        }


        /// <summary>
        /// 主题设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Themesetting_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Theme.xaml");
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemConfig_Click(object sender, RoutedEventArgs e)
        {
            JumpPage(@"/Views/Bodys/Theme.xaml");
        }






























        /// <summary>
        /// 使用Frame跳转页面
        /// </summary>
        private void JumpPage(string uid)
        {
            if (!String.IsNullOrWhiteSpace(uid))
            {
                this.mainFrame.Navigate(new Uri(uid, UriKind.Relative));
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IndexExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //退出程序
        }
    }
}
