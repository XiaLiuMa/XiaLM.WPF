using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using XiaLM.WPF.T01.Db;

namespace XiaLM.WPF.T01.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 重写程序启动事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            MainRaalize.GetInstance().DbInit();   //初始化数据库
            MainRaalize.GetInstance().UiInit(); //初始化UI
        }
    }
}
