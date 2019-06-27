
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

namespace MaxRobotServerApp.Views.Pages
{
    /// <summary>
    /// SystemServerConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class SystemServerConfigPage : Page
    {
        public SystemServerConfigPage()
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

            AlarmmanagerDal dal = new AlarmmanagerDal();
            var parms = dal.GetAlarmConfig();
            this.t2alarmDeleteEnable.IsChecked = parms.Enable;
            this.t2alarmDeleteTime.Text = parms.TotalMilliseconds.ToString();
        }

        /// <summary>
        /// 保存报警配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2SaveAlarmConfig_Click(object sender, RoutedEventArgs e)
        {
            AlarmmanagerDal dal = new AlarmmanagerDal();
            AlarmConfigParms parms = new AlarmConfigParms()
            {
                Enable = (bool)this.t2alarmDeleteEnable.IsChecked,
                TotalMilliseconds = Convert.ToDouble(this.t2alarmDeleteTime.Text.Trim())
            };
            bool flag = dal.SetAlarmConfig(parms);
            string txt = flag ? "保存成功！" : "保存失败。。。";
            Warning warning = new Warning(txt);
            warning.ShowDialog();
        }
    }
}
