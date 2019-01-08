using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
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
using System.Windows.Shapes;
using XiaLM.WPF.T01.App.Models;
using XiaLM.WPF.T01.App.UserControls;
using XiaLM.WPF.T01.Db.Manaments;
using XiaLM.WPF.T01.Utility;
using XiaLM.WPF.T01.Utility.Windows;

namespace XiaLM.WPF.T01.App.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string pw = PasswordBox.Password;

            #region 登陆等待
            var loadDialog = new LoadDialog();
            var result = DialogHost.Show(loadDialog, "LoginDialog", delegate (object sed, DialogOpenedEventArgs args)
            {
                bool flag = UserManament.GetInstance().UserLogin(new Db.UiModels.UserLoginData()
                {
                    Name = name,
                    Pwd = pw
                });
                args.Session.Close(false);
                if (flag)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageTips("登陆失败");
                }

                //Task.Factory.StartNew(() =>
                //{
                //    string url = $"https://api.bobdong.cn/time_manager/user/login?name={name}&pw={pw}";
                //    var resultStr = HttpHelper.Get(url);
                //    var resultObj = JsonConvert.DeserializeObject<ReturnData<User>>(resultStr);
                //    this.BeginInvoke(delegate ()// 异步更新界面
                //    {
                //        args.Session.Close(false);
                //        if (resultObj.code != 0)
                //        {
                //            MessageTips(resultObj.message);
                //        }
                //        else
                //        {
                //            //MainStaticData.AccessToken = resultObj.data.access_token;
                //            this.Close();
                //        }
                //    });
                //});
            });
            #endregion

        }

        /// <summary>
        /// 进入注册布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignIn_click(object sender, RoutedEventArgs e)
        {
            double AnimationTime = 0.7;
            GridOperator.Fade_Out_Grid(LoginGrid, AnimationTime);
            GridOperator.Fade_Int_Grid(SignInGrid, AnimationTime);
        }

        public async void MessageTips(string message)
        {
            var tipDialog = new TipDialog { Message = { Text = message } };
            await DialogHost.Show(tipDialog, "LoginDialog");
        }


        private void LoginWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key==Key.Enter)
            //{
            //    this.Login_click(sender,e);
            //}
        }

        private void PasswordBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Login_click(sender, e);
            }
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/d100000/worktimemanage");
        }

        /// <summary>
        /// 返回登陆布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            double AnimationTime = 0.7;
            GridOperator.Fade_Out_Grid(SignInGrid, AnimationTime);
            GridOperator.Fade_Int_Grid(LoginGrid, AnimationTime);
        }

        /// <summary>
        /// 注册新账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignInNew_OnClick(object sender, RoutedEventArgs e)
        {
            if (SignInPassWord.Password != SignInConfimPassWord.Password)
            {
                MessageTips("密码不一致！");
                return;
            }
            string name = SignInUserName.Text;
            string pw = SignInPassWord.Password;

            #region 注册等待
            var loadDialog = new LoadDialog();
            var result = DialogHost.Show(loadDialog, "LoginDialog", delegate (object sed, DialogOpenedEventArgs args)
            {
                bool flag = UserManament.GetInstance().UserSign(new Db.UiModels.UseSignData()
                {
                    Name = name,
                    Pwd = pw
                });
                args.Session.Close(false);
                if (flag)
                {
                    return;
                }
                else
                {
                    MessageTips("注册失败");
                }


                //Task.Factory.StartNew(() =>
                //{
                //    string url = $"https://api.bobdong.cn/time_manager/user/register?name={name}&pw={pw}";
                //    var resultStr = HttpHelper.Get(url);
                //    var resultObj = JsonConvert.DeserializeObject<ReturnData<User>>(resultStr);
                //    this.BeginInvoke(delegate ()// 异步更新界面
                //    {
                //        args.Session.Close(false);
                //        if (resultObj.code != 0)
                //        {
                //            MessageTips(resultObj.message);
                //        }
                //        else
                //        {
                //            //MainStaticData.AccessToken = resultObj.data.access_token;
                //            Close();
                //        }
                //    });
                //});
            });
            #endregion
        }

    }
}
