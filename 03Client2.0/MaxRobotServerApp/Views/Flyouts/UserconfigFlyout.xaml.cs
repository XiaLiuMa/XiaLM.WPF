using MyCustomControl;

using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Extensions.Comm;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// UserconfigFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class UserconfigFlyout : CBaseFlyout
    {
        /// <summary>
        /// 是否是编辑模式，否则是添加模式
        /// </summary>
        private bool IsEdit { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        private UserType usertype
        {
            get
            {
                UserType type = UserType.User;
                switch (this.updatetypeComBox.SelectedIndex)
                {
                    case 1:
                        type = UserType.Admin;
                        break;
                    default:
                        type = UserType.User;
                        break;
                }
                return type;
            }
        }

        /// <summary>
        /// 提交成功事件
        /// </summary>
        public event Action ConfirmSuccess = () => { };

        public UserconfigFlyout(Userinfo obj)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit(obj);
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInit(Userinfo obj)
        {
            if (obj != null)
            {
                IsEdit = true;
                this.username.Text = obj.Name;
                this.password.Text = obj.Password;
                switch (obj.UserType)
                {
                    case "管理员":
                        this.updatetypeComBox.SelectedIndex = 1;
                        break;
                    default:
                        this.updatetypeComBox.SelectedIndex = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.username.Text.Trim())) return;  //未填用户名
            try
            {
                UserManageDal dal = new UserManageDal();
                UserParms parms = new UserParms()
                {
                    Name = this.username.Text.Trim(),
                    Password = this.password.Text.Trim()
                };

                if (IsEdit)
                {
                    bool flag = dal.Updata(parms);
                    if (flag)
                    {
                        ConfirmSuccess();
                    }
                    else
                    {
                        new Warning("编辑用户失败!").ShowDialog();
                    }
                }
                else
                {
                    bool flag = dal.Add(parms);
                    if (flag)
                    {
                        ConfirmSuccess();
                    }
                    else
                    {
                        new Warning("添加用户失败!").ShowDialog();
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
