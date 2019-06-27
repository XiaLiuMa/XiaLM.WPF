
using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk.Model;
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
    /// AlarmBlackListPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmBlackListPage : Page
    {
        public AlarmBlackListPage()
        {
            InitializeComponent();
            this.DataContext = new AlarmBlacklistViewModel();
            this.t6pager.SelectionChangedEvent += Pager6_SelectionChangedEvent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    string tag = this.t6robotTag.Text.Trim();
                    string sTime = this.t6startTime.UDateTime;
                    string eTime = this.t6endTime.UDateTime;
                    (this.DataContext as AlarmBlacklistViewModel).RefreshDataGrid(tag, sTime, eTime);
                }));
            });
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager6_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as AlarmBlacklistViewModel).ChangePageSize(pageSize);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T6SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.t6robotTag.Text.Trim();
            string sTime = this.t6startTime.UDateTime;
            string eTime = this.t6endTime.UDateTime;
            (this.DataContext as AlarmBlacklistViewModel).RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T6DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除黑名单报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = t6dataGrid.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (t6dataGrid.SelectedItems[i] as BlacklistAlarm).Id;
                }

                AlarmmanagerDal dal = new AlarmmanagerDal();
                bool flag = dal.DeleteBlacklistAlarm(ids);
                if (flag)
                {
                    (this.DataContext as AlarmBlacklistViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除黑名单报警失败!").ShowDialog();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T6SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t6dataGrid.Items.Count; i++)
            {
                var cntr = t6dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T6SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t6dataGrid.Items.Count; i++)
            {
                var cntr = t6dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }

        /// <summary>
        /// 查看黑名单现场图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T6xct_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as BlacklistAlarm;
            new ShowImgFlyout(obj.CaptureImgUrl).ShowDialog();
        }

        /// <summary>
        /// 查看黑名单原始图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T6yst_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as BlacklistAlarm;
            new ShowImgFlyout(obj.OriginalImgUrl).ShowDialog();
        }
    }
}
