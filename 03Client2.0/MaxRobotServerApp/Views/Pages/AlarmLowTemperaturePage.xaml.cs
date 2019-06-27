﻿
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
    /// AlarmLowTemperaturePage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmLowTemperaturePage : Page
    {
        public AlarmLowTemperaturePage()
        {
            InitializeComponent();
            this.DataContext = new AlarmLowTemperatureViewModel();
            this.t2pager.SelectionChangedEvent += Pager2_SelectionChangedEvent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    string tag = this.t2robotTag.Text.Trim();
                    string sTime = this.t2startTime.UDateTime;
                    string eTime = this.t2endTime.UDateTime;
                    (this.DataContext as AlarmLowTemperatureViewModel).RefreshDataGrid(tag, sTime, eTime);
                }));
            });
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager2_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as AlarmLowTemperatureViewModel).ChangePageSize(pageSize);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.t2robotTag.Text.Trim();
            string sTime = this.t2startTime.UDateTime;
            string eTime = this.t2endTime.UDateTime;
            (this.DataContext as AlarmLowTemperatureViewModel).RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除低温探测报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = t2dataGrid.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (t2dataGrid.SelectedItems[i] as LowTemperatureAlarm).Id;
                }

                AlarmmanagerDal dal = new AlarmmanagerDal();
                bool flag = dal.DeleteLowTemperatureAlarm(ids);
                if (flag)
                {
                    (this.DataContext as AlarmLowTemperatureViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除低温探测报警失败!").ShowDialog();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t2dataGrid.Items.Count; i++)
            {
                var cntr = t2dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T2SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t2dataGrid.Items.Count; i++)
            {
                var cntr = t2dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }

        /// <summary>
        /// 查看低温探测现场图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2xct_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as LowTemperatureAlarm;
            new ShowImgFlyout(obj.LiveImgUrl).ShowDialog();
        }

        /// <summary>
        /// 查看低温探测红外图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2hwt_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as LowTemperatureAlarm;
            new ShowImgFlyout(obj.TempImgUrl).ShowDialog();
        }
    }
}
