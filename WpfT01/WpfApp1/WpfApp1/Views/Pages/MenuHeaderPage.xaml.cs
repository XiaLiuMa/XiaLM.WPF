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

namespace WpfApp1.Views.Pages
{
    /// <summary>
    /// MenuHeaderPage.xaml 的交互逻辑
    /// </summary>
    public partial class MenuHeaderPage : Page
    {
        /// <summary>
        /// 菜单标题页
        /// </summary>
        public MenuHeaderPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            menuGrid.Children.Clear();
            List<MenuInfo> lst = MenuManager.GetInstance().ChildMenus;
            for (int i = 0; i < lst.Count; i++)
            {
                menuGrid.ColumnDefinitions.Add(new ColumnDefinition());
                Button btu = new Button()
                {
                    Tag = lst[i].Tag,
                    Content = lst.Where(p => p.Tag.Equals(lst[i].Tag)).FirstOrDefault().Name,
                };
                btu.Click += Btu_Click;
                menuGrid.Children.Add(btu);
                Grid.SetColumn(btu, i);
            }
        }

        private void Btu_Click(object sender, RoutedEventArgs e)
        {
            var window = (MainWindow)Window.GetWindow(this);
            window.SelectCrumbs_Click(sender, e);
        }
    }
}
