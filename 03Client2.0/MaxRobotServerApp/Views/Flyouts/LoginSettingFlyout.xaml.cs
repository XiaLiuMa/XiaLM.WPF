using System;
using System.Windows;
using MaxRobotServerApp.Extensions.Comm;
using MaxRobotServerApp.Mods.Config;
using MyCustomControl;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// LoginSettingFlyout .xaml 的交互逻辑
    /// </summary>
    public partial class LoginSettingFlyout : CBaseFlyout
    {
        /// <summary>
        /// 登陆设置
        /// </summary>
        /// <param name="ip">报警信息</param>
        /// <param name="port"></param>
        public LoginSettingFlyout()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadConfig();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadConfig()
        {
            SystemConfig config = ConfigManager.GetInstance().LoadSystemConfig();
            if (config != null)
            {
                this.loginIP.Text = config.ServerIp;
                this.loginPORT.Text = config.ServerPort.ToString();
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private void SaveConfig()
        {
            SystemConfig config = new SystemConfig()
            {
                ServerIp = this.loginIP.Text.Trim(),
                ServerPort = Convert.ToInt32(this.loginPORT.Text.Trim())
            };
            bool flag = ConfigManager.GetInstance().SaveSystemConfig(config);
            if (flag)
            {
                this.Close();
            }
            else
            {
                new Warning("保存失败。。。").ShowDialog();
            }
        }
    }
}
