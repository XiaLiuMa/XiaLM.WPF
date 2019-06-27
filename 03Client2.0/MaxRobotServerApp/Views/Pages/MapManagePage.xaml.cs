
using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.Flyouts;
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
    /// IndexPage.xaml 的交互逻辑
    /// </summary>
    public partial class MapManagePage : Page
    {
        public MapManagePage()
        {
            InitializeComponent();
            this.DataContext = new MapManagerViewModel();
            this.pager1.SelectionChangedEvent += Pager1_SelectionChangedEvent;
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

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager1_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as MapManagerViewModel).PageSize = pageSize;
            (this.DataContext as MapManagerViewModel).RefreshDataGrid();
        }

        //添加
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var mapInfoFlyout = new MapInfoFlyout();
            mapInfoFlyout.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as MapManagerViewModel).RefreshDataGrid();
                    mapInfoFlyout.Close();
                }));
            };
            mapInfoFlyout.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mapInfoFlyout.ShowDialog();
        }
        //删除
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int count = dataGrid1.SelectedItems.Count;
            if (count <= 0) return;
            int[] ids = new int[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = (dataGrid1.SelectedItems[i] as MapResource).Id;
            }
            var taskdal = new TaskMangerDal();
            var p = 0;
            foreach (var item in ids)
            {
                var map = (dataGrid1.SelectedItems[p] as MapResource);
                var b = taskdal.IsMapUsed(map.MapId);
                if (b)
                {
                    MessageBox.Show($"地图({map.Name})已在任务中配置，如需删除请先解除关联！");
                    return;
                }
                p++;
            }
            var dal = new MapResourceDal();
            bool flag = dal.DeleteMapInfo(ids);
            if (flag)
            {
                (this.DataContext as MapManagerViewModel).RefreshDataGrid();
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.dataGrid1.Items.Count; i++)
            {
                var cntr = dataGrid1.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = true;
                }
            }
        }
        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.dataGrid1.Items.Count; i++)
            {
                var cntr = dataGrid1.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }

        private void TbControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
                e.Handled = true;
        }

        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1tp_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as MapResource;
            new MapBrowserFlyout(obj.MapId).ShowDialog();
        }
        //编辑
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var mod = ((Image)sender).DataContext as MapResource;
            var mapInfoFlyout = new MapInfoFlyout(mod);
            mapInfoFlyout.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as MapManagerViewModel).RefreshDataGrid();
                    mapInfoFlyout.Close();
                }));
            };
            mapInfoFlyout.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mapInfoFlyout.ShowDialog();
        }
    }
}
