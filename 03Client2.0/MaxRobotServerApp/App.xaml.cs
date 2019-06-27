using MaxRobotServerApp.Views;
using System;
using System.Windows;

namespace MaxRobotServerApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 重写启动项
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new LoginWindow();
            MainWindow.Show();
        }
    }
}
