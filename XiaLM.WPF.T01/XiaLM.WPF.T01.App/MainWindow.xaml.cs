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
using XiaLM.WPF.T01.App.Windows;

namespace XiaLM.WPF.T01.App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //// 初始化登录窗口
            //LoginWindow loginWindow = new LoginWindow();
            //loginWindow.ShowDialog();
            //if (string.IsNullOrEmpty(MainStaticData.AccessToken))
            //{
            //    Close();
            //}
            //else
            //{
            //    InitializeComponent();

            //    DataContext = new MainWindowViewModel();

            //}
            //Init();
            //CheckUpdate();
            ////Tips("Welcome to WorkTimeManager!");

        }
    }
}
