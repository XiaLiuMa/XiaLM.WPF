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
using WpfApp1.Comm;
using WpfApp1.Mods;

namespace WpfApp1.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(this.menuGridB, 2);    //设置菜单栏为最高层
            Canvas.SetZIndex(this.menuBtu, 1);    //设置菜单按钮为第二高图层
            SelectedMenuRefresh(MenuManager.IndexTag);  //默认进入主页面
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
        /// 菜单选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMenu_Click(object sender, RoutedEventArgs e)
        {
            Label lb = sender as Label;
            SelectedMenuRefresh((string)lb.Tag);
        }

        /// <summary>
        /// 面包屑选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectCrumbs_Click(object sender, RoutedEventArgs e)
        {
            Button btu = sender as Button;
            SelectedMenuRefresh((string)btu.Tag);
        }

        /// <summary>
        /// 选中菜单后刷新
        /// </summary>
        /// <param name="tag"></param>
        private void SelectedMenuRefresh(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            #region page刷新
            string frmPath = MenuManager.GetInstance().Menus.Where(p => p.Tag.Equals(tag)).FirstOrDefault().Page;
            if (!string.IsNullOrEmpty(frmPath))
            {
                if (frmPath.Equals("MenuHeaderPage"))
                {
                    List<MenuInfo> lst1 = new List<MenuInfo>();
                    foreach (var item in MenuManager.GetInstance().Menus)
                    {
                        if (item.Tag.Contains(tag) && !item.Tag.Equals(tag))    //包含，但不等于
                        {
                            lst1.Add(item);
                        }
                    }
                    MenuManager.GetInstance().ChildMenus = lst1;
                }
                frmPath = $@"/Views/Pages/{frmPath}.xaml";
                this.frmMain.Navigate(new Uri(frmPath, UriKind.Relative));
            }
            #endregion

            #region 面包屑刷新
            List<string> taglst = new List<string>();
            for (int i = 0; i < (tag.Length / 2); i++)
            {
                taglst.Add(tag.Substring(0, (i + 1) * 2)); //字符数(2,4,6,8......)
            }
            this.crumbsStackPanel.Children.Clear();
            //面包屑中先增加“首页”
            Button btu = new Button()
            {
                Tag = MenuManager.IndexTag,
                Content = MenuManager.GetInstance().Menus.Where(p => p.Tag.Equals(MenuManager.IndexTag)).FirstOrDefault().Name,
                Width = 30,
                Style = (Style)this.FindResource("ButtonStyle1")
            };
            btu.Click += SelectCrumbs_Click;
            this.crumbsStackPanel.Children.Add(btu);
            //再增加菜单树
            foreach (string item in taglst)
            {
                if (!item.Equals(MenuManager.IndexTag)) //避免首页重复添加
                {
                    Button Mbtu = new Button()
                    {
                        Tag = item,
                        Content = MenuManager.GetInstance().Menus.Where(p => p.Tag.Equals(item)).FirstOrDefault().Name,
                        Width = 30,
                        Margin = new Thickness(5, 0, 0, 5),
                        Style = (Style)this.FindResource("ButtonStyle1")
                    };
                    Mbtu.Click += SelectCrumbs_Click;
                    this.crumbsStackPanel.Children.Add(Mbtu);
                }
            }
            #endregion
        }
    }
}
