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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 菜单字典
        /// </summary>
        private Dictionary<string, string> dic;
        /// <summary>
        /// 面包屑列表
        /// </summary>
        private Dictionary<string, string> menus;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.TestPageViewModel();

            #region 初始化字典列表
            dic = new Dictionary<string, string>();
            dic.Add("A1", "首页");
            dic.Add("A2", "用户管理");
            #endregion
            menus = new Dictionary<string, string>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(this.menuGridB, 2);    //设置菜单栏为最高层
            Canvas.SetZIndex(this.menuBtu, 1);    //设置菜单按钮为第二高图层
        }

        /// <summary>
        /// 鼠标移上菜单状态A
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGridA_MouseEnter(object sender, MouseEventArgs e)
        {
            this.menuGridB.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 鼠标移出菜单状态B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGridB_MouseLeave(object sender, MouseEventArgs e)
        {
            this.menuGridB.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 菜单选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMenu_Click(object sender, RoutedEventArgs e)
        {

            Label lb = sender as Label;
            string guid = lb.DataContext as string;
            string[] ids = guid.Split();

            foreach (string id in ids)
            {
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    if (id.Equals(kvp.Key))
                    {
                        menus.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            
        }

        /// <summary>
        /// 选中菜单
        /// </summary>
        private void SelectedMenu(string uid)
        {
            this.crumbsStackPanel.Children.Clear();
            Button btu1 = new Button()
            {
                Content = "首页",
                Width = 30,
                Style = (Style)this.FindResource("ButtonStyle1")
            };
            this.crumbsStackPanel.Children.Add(btu1);
        }
    }
}
