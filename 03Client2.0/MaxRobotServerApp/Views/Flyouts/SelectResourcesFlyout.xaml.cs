using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using MyCustomControl;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// SelectResourcesFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class SelectResourcesFlyout : CBaseFlyout
    {
        /// <summary>
        /// 选择成功事件
        /// </summary>
        public event Action<Preachresource> OnSelected = (p) => { };

        public SelectResourcesFlyout()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit();
        }

        /// <summary>
        /// 初始化View
        /// </summary>
        private void ViewInit()
        {
            this.grid1.DataContext = new PreachresourcesViewModel();
            this.pager1.SelectionChangedEvent += Pager1_SelectionChangedEvent;
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager1_SelectionChangedEvent(int pageSize)
        {
            (this.grid1.DataContext as PreachresourcesViewModel).PageSize = pageSize;
            (this.grid1.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            (this.grid1.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
        }

        /// <summary>
        /// 选中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedBtu_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Button)sender).DataContext as Preachresource;
            OnSelected(obj);
            this.Close();
        }
    }
}
