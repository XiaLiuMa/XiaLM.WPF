using System.Windows;
using MaxRobotServerApp.Extensions.Comm;
using MaxRobotServerApp.Dals;
using MyCustomControl;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// ChangepwdFlyout .xaml 的交互逻辑
    /// </summary>
    public partial class ChangepwdFlyout : CBaseFlyout
    {
        public ChangepwdFlyout(string user)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(user))
            {
                this.username.Text = user;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            string newpassword = this.newpassword.Text.Trim();
            string affirmnewpassword = this.affirmnewpassword.Text.Trim();
            if (!affirmnewpassword.Equals(newpassword))
            {
                new Warning("新密码不一致").ShowDialog();
            }
            else
            {
                UserManageDal dal = new UserManageDal();
                var flag = dal.Verify(new Maxvision.Robot.Sdk.Model.UserParms()
                {
                    Name = this.username.Text.Trim(),
                    Password = this.oledpassword.Text.Trim()
                });
                if (flag)
                {
                    var flag1 = dal.Updata(new Maxvision.Robot.Sdk.Model.UserParms()
                    {
                        Name = this.username.Text.Trim(),
                        Password = affirmnewpassword
                    });
                    if (flag1)
                    {
                        this.Close();
                        UserManager.GetInstance().RclientLoginOut();
                    }
                    else
                    {
                        new Warning("修改失败").ShowDialog();
                    }
                }
                else
                {
                    new Warning("旧密码不正确").ShowDialog();
                }
            }
        }

        
    }
}
