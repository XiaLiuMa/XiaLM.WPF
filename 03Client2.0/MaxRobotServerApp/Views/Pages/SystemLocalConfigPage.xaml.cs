
using MaxRobotServerApp.Extensions.Comm;
using MaxRobotServerApp.Mods.Config;
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

namespace MaxRobotServerApp.Views.Pages
{
    /// <summary>
    /// SystemLocalConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class SystemLocalConfigPage : Page
    {
        public SystemLocalConfigPage()
        {
            InitializeComponent();
            //this.DataContext = new AlarmBlacklistViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(() =>
            //{
            //    this.Dispatcher?.Invoke(new Action(() =>
            //    {
            //        string tag = this.t6robotTag.Text.Trim();
            //        string sTime = this.t6startTime.UDateTime;
            //        string eTime = this.t6endTime.UDateTime;
            //        (this.DataContext as AlarmBlacklistViewModel).RefreshDataGrid(tag, sTime, eTime);
            //    }));
            //});

            SystemConfig config = ConfigManager.GetInstance().LoadSystemConfig();
            if (config != null)
            {
                this.t1serverIp.Text = config.ServerIp;
                this.t1serverPort.Text = config.ServerPort.ToString();
            }
        }

        /// <summary>
        /// 保存本地配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            //SystemConfig config = new SystemConfig()
            //{
            //    ServerIp = this.t1serverIp.Text.Trim(),
            //    ServerPort = Convert.ToInt32(this.t1serverPort.Text.Trim())
            //};
            //bool flag = ConfigManager.GetInstance().SaveSystemConfig(config);
        }
    }
}
