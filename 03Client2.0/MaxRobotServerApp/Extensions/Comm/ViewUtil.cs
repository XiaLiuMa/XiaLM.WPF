using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace MaxRobotServerApp.Extensions.Comm
{
    public class ViewUtil
    {
        /// <summary>
        /// 使用NavigationWindow弹出页面
        /// </summary>
        /// <param name="title"></param>
        /// <param name="uri"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void ShowPage(string title, string uri, double height = 480, double width = 600)
        {
            NavigationWindow window = new NavigationWindow();
            window.Title = title;
            window.Width = width;
            window.Height = height;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ResizeMode = ResizeMode.NoResize;
            window.Source = new Uri(uri, UriKind.Relative);
            window.ShowsNavigationUI = false;
            window.Show();
        }
    }
}
