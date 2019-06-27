using CefSharp;

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
    /// RobotMapPage.xaml 的交互逻辑
    /// </summary>
    public partial class RobotMapPage : Page
    {
        public RobotMapPage()
        {
            InitializeComponent();
            Task.Factory.StartNew(async () =>
            {
                this.Dispatcher?.Invoke(() =>
                {
                    CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                    this.mapBrowser.RegisterJsObject("jsObj", new MapBrowserViewModel(), BindingOptions.DefaultBinder);
                    this.mapBrowser.Address = (AppDomain.CurrentDomain.BaseDirectory + @"html\map\MapView.html");
                });
            });
            RobotPositon.Builder.Init();
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
        }

        private int num = 0;
        private void TabItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (num == 0)
            {
                this.mapBrowser.Reload();
                this.mapBrowser.ShowDevTools();
                num++;
            }
        }
    }
}
