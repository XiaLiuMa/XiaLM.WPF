using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaxRobotServerApp.Extensions.Comm;
using MaxRobotServerApp.Dals.LocalDal;
using MaxRobotServerApp.Mods.Vmod;
using MaxRobotServerApp.Views.Flyouts;
using System.Windows.Media.Animation;
using System.Linq;

namespace MaxRobotServerApp.Views
{
    /// <summary>
    /// LoginWindow .xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            InitView();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;    //起始值
            da.To = 1;      //结束值
            da.Duration = TimeSpan.FromSeconds(0.75);         //动画持续时间
            this.BeginAnimation(Window.OpacityProperty, da);

            //关键帧定义
            DoubleAnimationUsingKeyFrames dak = new DoubleAnimationUsingKeyFrames();
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(100, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.15))));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(200, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3))));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(300, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.45))));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(400, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
            dak.KeyFrames.Add(new LinearDoubleKeyFrame(this.ActualHeight, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.75))));
            this.BeginAnimation(Window.HeightProperty, dak);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        private List<LoginRecord> users = new List<LoginRecord>();
        private void InitView()
        {
            LoginRecordDal dal = new LoginRecordDal();
            users = dal.FuzzyQuery();
            if (users != null && users.Count > 0)
            {
                List<string> unames = new List<string>();   //用户名称列表
                foreach (var item in users)
                {
                    unames.Add(item.uname);
                }
                this.username.DropItems = unames;
                this.username.Text = unames.FirstOrDefault();

                LoginRecord obj = users.Where(p => p.uname.Equals(this.username.Text.Trim())).FirstOrDefault();
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.uname))
                    {
                        if (!string.IsNullOrEmpty(obj.passwrd))
                        {
                            this.password.Password = obj.passwrd;
                            this.logintype.SelectedIndex = 2;   //记住用户名和密码
                        }
                        else
                        {
                            this.logintype.SelectedIndex = 1;   //记住用户名
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 用户名输入变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void username_TextChanged(object sender, RoutedEventArgs e)
        {
            if (users != null && users.Count > 0)
            {
                List<string> unames = new List<string>();   //用户名称列表
                foreach (var item in users)
                {
                    unames.Add(item.uname);
                }
                this.username.DropItems = unames.Where(p => p.Contains(this.username.Text.Trim())).ToArray();
                LoginRecord obj = users.Where(p => p.uname.Equals(this.username.Text.Trim())).FirstOrDefault();
                if (obj != null)
                {
                    if (!string.IsNullOrEmpty(obj.uname))
                    {
                        if (!string.IsNullOrEmpty(obj.passwrd))
                        {
                            this.password.Password = obj.passwrd;
                            this.logintype.SelectedIndex = 2;   //记住用户名和密码
                        }
                        else
                        {
                            this.logintype.SelectedIndex = 1;   //记住用户名
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            LoginSettingFlyout flyout = new LoginSettingFlyout();
            flyout.ShowDialog();
        }

        

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.loading.Visibility = Visibility.Visible;   //显示
            string user = this.username.Text.Trim();
            string pwd = this.password.Password.Trim();
            Task.Factory.StartNew(() =>
            {
                var flag = UserManager.GetInstance().RclientLogin(user, pwd);
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    this.loading.Visibility = Visibility.Collapsed;   //隐藏
                    if (flag)
                    {
                        LoginRecordDal dal = new LoginRecordDal();
                        LoginRecord obj = new LoginRecord();
                        switch (this.logintype.SelectedIndex)
                        {
                            case 0: //临时登陆
                                {
                                    obj = dal.GetInfo(user);
                                    if (obj != null)
                                    {
                                        dal.Delete(new string[] { obj.uname });
                                    }
                                }
                                break;
                            case 1: //记住用户名
                                {
                                    obj.uname = user;
                                    obj.passwrd = "";
                                    obj.logintime = DateTime.Now.ToString();

                                    var t1obj = dal.GetInfo(user);
                                    if (t1obj != null)  //修改
                                    {
                                        dal.Update(obj);
                                    }
                                    else    //新增
                                    {
                                        dal.Add(obj);
                                    }
                                }
                                break;
                            case 2: //记住用户名和密码
                                {
                                    obj.uname = user;
                                    obj.passwrd = pwd;
                                    obj.logintime = DateTime.Now.ToString();

                                    var t1obj = dal.GetInfo(user);
                                    if (t1obj != null)  //修改
                                    {
                                        dal.Update(obj);
                                    }
                                    else    //新增
                                    {
                                        dal.Add(obj);
                                    }
                                }
                                break;
                        }
                        new MainWindow().Show();
                        this.Close();
                    }
                    else
                    {
                        new Warning("登陆失败！").ShowDialog();
                    }
                }));
            });
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginExit_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); //退出程序
        }

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
