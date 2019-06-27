
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
    /// AlarmAreaMonitoringPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmAreaMonitoringPage : Page
    {
        public AlarmAreaMonitoringPage()
        {
            InitializeComponent();
            this.DataContext = new AlarmAreaMonitoringViewModel();
            this.t1pager.SelectionChangedEvent += Pager1_SelectionChangedEvent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    string tag = this.t1robotTag.Text.Trim();
                    string sTime = this.t1startTime.UDateTime;
                    string eTime = this.t1endTime.UDateTime;
                    (this.DataContext as AlarmAreaMonitoringViewModel).RefreshDataGrid(tag, sTime, eTime);
                }));
            });
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager1_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as AlarmAreaMonitoringViewModel).ChangePageSize(pageSize);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.t1robotTag.Text.Trim();
            string sTime = this.t1startTime.UDateTime;
            string eTime = this.t1endTime.UDateTime;
            (this.DataContext as AlarmAreaMonitoringViewModel).RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除区域监控报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = t1dataGrid.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (t1dataGrid.SelectedItems[i] as AreaMonitoringAlarm).Id;
                }
                AlarmmanagerDal dal = new AlarmmanagerDal();
                bool flag = dal.DeleteAreaMonitoringAlarm(ids);
                if (flag)
                {
                    (this.DataContext as AlarmAreaMonitoringViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除区域监控报警失败!").ShowDialog();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t1dataGrid.Items.Count; i++)
            {
                var cntr = t1dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T1SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t1dataGrid.Items.Count; i++)
            {
                var cntr = t1dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }

        /// <summary>
        /// 查看区域监控现场图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1xct_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as AreaMonitoringAlarm;
            new ShowImgFlyout(obj.ImgUrl).ShowDialog();
        }
    }

}
