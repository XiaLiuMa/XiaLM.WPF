
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
    /// AlarmFaceContrastPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmFaceContrastPage : Page
    {
        public AlarmFaceContrastPage()
        {
            InitializeComponent();
            this.DataContext = new AlarmFaceContrastViewModel();
            this.t3pager.SelectionChangedEvent += Pager3_SelectionChangedEvent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    string tag = this.t3robotTag.Text.Trim();
                    string sTime = this.t3startTime.UDateTime;
                    string eTime = this.t3endTime.UDateTime;
                    (this.DataContext as AlarmFaceContrastViewModel).RefreshDataGrid(tag, sTime, eTime);
                }));
            });
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager3_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as AlarmFaceContrastViewModel).ChangePageSize(pageSize);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T3SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.t3robotTag.Text.Trim();
            string sTime = this.t3startTime.UDateTime;
            string eTime = this.t3endTime.UDateTime;
            (this.DataContext as AlarmFaceContrastViewModel).RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T3DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除人脸识别报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = t3dataGrid.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (t3dataGrid.SelectedItems[i] as FaceContrastAlarm).Id;
                }

                AlarmmanagerDal dal = new AlarmmanagerDal();
                bool flag = dal.DeleteFaceContrastAlarm(ids);
                if (flag)
                {
                    (this.DataContext as AlarmFaceContrastViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除人脸识别报警失败!").ShowDialog();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T3SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t3dataGrid.Items.Count; i++)
            {
                var cntr = t3dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T3SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t3dataGrid.Items.Count; i++)
            {
                var cntr = t3dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }

        /// <summary>
        /// 查看人脸对比现场图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T3xct_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as FaceContrastAlarm;
            new ShowImgFlyout(obj.CaptureImgUrl).ShowDialog();
        }

        /// <summary>
        /// 查看人脸对比证件图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T3zjt_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as FaceContrastAlarm;
            new ShowImgFlyout(obj.IDImgUrl).ShowDialog();
        }
    }
}
