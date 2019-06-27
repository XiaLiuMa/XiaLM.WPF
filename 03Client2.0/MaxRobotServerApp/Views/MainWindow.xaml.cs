using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using MaxRobotServerApp.Extensions.Comm;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.Infos;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;

namespace MaxRobotServerApp.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int alarmCount = 0; //新增报警次数
        private Grid selectedMenu { get; set; }
        private List<AlarmPush> alarms { get; set; }

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit();
            this.DataContext = new MainWindowViewModel();
        }

        private void ViewInit()
        {
            alarms = new List<AlarmPush>();
            AlarmManagement alarm = new AlarmManagement();
            alarm.OnAlarmInfo += Alarm_OnAlarmInfo;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    this.mainFrame.Navigate(new Uri(@"/Views/Pages/IndexPage.xaml", UriKind.Relative));
                }));
            });
        }

        /// <summary>
        /// 报警消息推送事件
        /// </summary>
        /// <param name="alarm"></param>
        private void Alarm_OnAlarmInfo(AlarmPush alarm)
        {
            if (alarms.Count >= 20) alarms.RemoveAt(19);    //列表最多保存20个
            alarms.Insert(0, alarm); //往第一个插入

            this.Dispatcher?.Invoke(new Action(() =>
            {
                if (!this.alarmGrid.Visibility.Equals(Visibility.Visible))   //右侧边框关闭的时候才追加报警数量
                {
                    alarmCount++;
                    this.alarmcount.Content = (alarmCount > 9) ? "9+" : alarmCount.ToString();
                }
                RefreshAlarmList(alarms);    //刷新报警列表到右侧边框
            }));
        }

        /// <summary>
        /// 刷新报警列表
        /// </summary>
        private void RefreshAlarmList(List<AlarmPush> lst)
        {
            this.alarmStackPanel.Children.Clear();
            if (lst == null || lst.Count <= 0) return;
            for (int i = 0; i < lst.Count; i++)
            {
                UAlarmInfo alarmInfo = new UAlarmInfo(new UAlarmInfoViewModel(lst[i]));
                this.alarmStackPanel.Children.Add(alarmInfo);
            }
        }

        #region 工具栏
        /// <summary>
        /// 鼠标移上事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbar_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666669"));
        }
        /// <summary>
        /// 鼠标移出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toolbar_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2DC049"));
        }

        /// <summary>
        /// 快捷按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickBtu_Click(object sender, RoutedEventArgs e)
        {
            this.quickPopup.IsOpen = false;
            this.quickPopup.IsOpen = true;
        }

        /// <summary>
        /// 报警消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlarmInfo_Click(object sender, RoutedEventArgs e)
        {
            this.alarmGrid.Visibility = Visibility.Visible;
            this.alarmcount.Content = "";   //清空报警数量提示
            alarmCount = 0;
        }

        /// <summary>
        /// 隐藏报警消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HiddenAlarmInfo_Click(object sender, RoutedEventArgs e)
        {
            this.alarmGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 系统最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 系统最大化切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.maxImga.Visibility = Visibility.Hidden;
                this.maxImgb.Visibility = Visibility.Visible;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.maxImga.Visibility = Visibility.Visible;
                this.maxImgb.Visibility = Visibility.Hidden;
            }
        }


        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //退出程序
        }
        #endregion


        #region 快捷按钮
        /// <summary>
        /// 鼠标移上事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickItem_MouseEnter(object sender, MouseEventArgs e)
        {
            var lb = sender as Label;
            lb.FontWeight = FontWeights.Bold;
            lb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2DC049"));
        }
        /// <summary>
        /// 鼠标移出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickItem_MouseLeave(object sender, MouseEventArgs e)
        {
            var lb = sender as Label;
            lb.FontWeight = FontWeights.Normal;
            lb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"));
        }


        /// <summary>
        /// 下载管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadManage_Click(object sender, RoutedEventArgs e)
        {
            DownloadFlyout.GetInstance().Show();
        }
        #endregion

        #region 菜单
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Changepwd_Click(object sender, RoutedEventArgs e)
        {
            ChangepwdFlyout flyout = new ChangepwdFlyout(UserManager.GetInstance().User.Name);
            flyout.ShowDialog();
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginoutBtn_Click(object sender, RoutedEventArgs e)
        {
            UserManager.GetInstance().RclientLoginOut();
            new LoginWindow().Show();
            this.Close();
        }

        /// <summary>
        /// 鼠标移上菜单按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.menuGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 鼠标移出菜单栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.menuGrid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 菜单展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            foreach (Expander exp in this.menuStackPanel.Children)
            {
                if (exp == null) continue;
                exp.IsExpanded = exp.Equals(sender as Expander) ? true : false; //只展开一个

                if (exp.Equals(sender as Expander)) //修改选中的样式
                {
                    foreach (var itm1 in (exp.Header as StackPanel).Children)
                    {
                        if ((itm1 as Image) != null)    //修改图片
                        {
                            MenuInfo menu = MenuManager.GetInstance().Menus.Where(p => p.Tag.Equals((exp.Header as StackPanel).Tag)).FirstOrDefault();
                            if (menu != null && !string.IsNullOrEmpty(menu.Img))
                            {
                                (itm1 as Image).Source = new BitmapImage(new Uri($@"pack://application:,,,/Views/Imgs/{menu.Img}ON.png"));
                            }
                        }
                        if ((itm1 as Label) != null)    //修改文字样式
                        {
                            (itm1 as Label).Style = (Style)this.FindResource("menu1Label");
                        }
                    }
                }
                else    //修改未选中的样式
                {
                    foreach (var itm2 in (exp.Header as StackPanel).Children)
                    {
                        if ((itm2 as Image) != null)    //修改图片
                        {
                            MenuInfo menu = MenuManager.GetInstance().Menus.Where(p => p.Tag.Equals((exp.Header as StackPanel).Tag)).FirstOrDefault();
                            if (menu != null && !string.IsNullOrEmpty(menu.Img))
                            {
                                (itm2 as Image).Source = new BitmapImage(new Uri($@"pack://application:,,,/Views/Imgs/{menu.Img}OFF.png"));
                            }
                        }
                        if ((itm2 as Label) != null)    //修改文字样式
                        {
                            (itm2 as Label).Style = (Style)this.FindResource("menu1LabelOff");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 菜单选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMenu_Click(object sender, RoutedEventArgs e)
        {
            Label lb = sender as Label;
            if (lb == null) return;
            foreach (Expander exp in this.menuStackPanel.Children)
            {
                if (exp == null) continue;
                StackPanel sp = exp.Content as StackPanel;
                if (sp != null)
                {
                    foreach (Label itm1 in sp.Children)
                    {
                        if (itm1 == null || itm1.Tag == null) continue;
                        if (itm1.Tag.Equals(lb.Tag))
                        {
                            (itm1 as Label).Style = (Style)this.FindResource("menu2Button");
                        }
                        else
                        {
                            (itm1 as Label).Style = (Style)this.FindResource("menu2ButtonOff");
                        }
                    }
                }
            }

            SelectedMenuRefresh((string)lb.Tag);
        }

        /// <summary>
        /// 首页按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Index_Click(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            Expander_Expanded(sender, e);
            SelectMenu_Click(new Label() { Tag = (string)img.Tag }, e);
            SelectedMenuRefresh((string)img.Tag);
        }

        /// <summary>
        /// 选中菜单后刷新
        /// </summary>
        /// <param name="tag"></param>
        private void SelectedMenuRefresh(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            #region page刷新
            string frmPath = MenuManager.GetInstance().Menus.Where(p => p.Tag.Equals(tag)).FirstOrDefault().Page;
            if (!string.IsNullOrEmpty(frmPath) && !tag.Equals(MenuManager.IndexTag))
            {
                if (frmPath.Equals("MenuHeaderPage"))
                {
                    List<MenuInfo> lst1 = new List<MenuInfo>();
                    foreach (var item in MenuManager.GetInstance().Menus)
                    {
                        if (item.Tag.Contains(tag) && !item.Tag.Equals(tag))    //包含，但不等于
                        {
                            lst1.Add(item);
                        }
                    }
                    MenuManager.GetInstance().ChildMenus = lst1;
                }
                frmPath = $@"/Views/Pages/{frmPath}.xaml";
                this.backFrame.Navigate(new Uri(frmPath, UriKind.Relative));
            }

            if (temTag.Equals(MenuManager.IndexTag))
            {
                if (!tag.Equals(MenuManager.IndexTag))
                {
                    Rotating();
                    IsClockwise = !IsClockwise;
                }
            }
            else
            {
                if (tag.Equals(MenuManager.IndexTag))
                {
                    Rotating();
                    IsClockwise = !IsClockwise;
                }
            }
            temTag = tag;
            #endregion
        }

        /// <summary>
        /// 当前显示的page
        /// </summary>
        private string temTag = MenuManager.IndexTag;
        /// <summary>
        /// 是否是顺时针翻转
        /// </summary>
        private bool IsClockwise = false;
        /// <summary>
        /// 3D翻转
        /// </summary>
        private void Rotating()
        {
            DoubleAnimation da = new DoubleAnimation();
            if (IsClockwise)
            {
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.To = 0;
                this.aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            }
            else
            {
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.To = 180;
                this.aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            }
        }

        /// <summary>
        /// 菜单切换
        /// </summary>
        /// <param name="sender"></param>
        private void MenuSelectedChange(object sender)
        {
            Grid grid = sender as Grid;
            if (grid != null)
            {
                foreach (var item in grid.Children)
                {
                    if (item.GetType() == typeof(Image))
                    {
                        Image img = item as Image;
                        if (img.Visibility == Visibility.Visible)
                        {
                            img.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            img.Visibility = Visibility.Visible;
                        }
                    }
                    if (item.GetType() == typeof(Label))
                    {
                        Label lb = item as Label;
                        lb.FontWeight = FontWeights.Bold;
                        lb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2DC049"));
                    }
                }
            }

            foreach (var item in selectedMenu.Children)
            {
                if (item.GetType() == typeof(Image))
                {
                    Image img = item as Image;
                    if (img.Visibility == Visibility.Visible)
                    {
                        img.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        img.Visibility = Visibility.Visible;
                    }
                }
                if (item.GetType() == typeof(Label))
                {
                    Label lb = item as Label;
                    lb.FontWeight = FontWeights.Normal;
                    lb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"));
                }
            }

            selectedMenu = grid;
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserManager_Click(object sender, RoutedEventArgs e)
        {
            //MenuSelectedChange(sender);
            //switch (UserManager.GetInstance().User.UserType)
            //{
            //    case UserType.Admin:
            //        {
            //            JumpPage(@"/Views/Bodys/UserConfig.xaml");
            //        }
            //        break;
            //    case UserType.User:
            //        {
            //            JumpPage(@"/Views/Bodys/WithoutPermission.xaml");   //无权限
            //        }
            //        break;
            //}
        }

        #endregion


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
