
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
    /// AlarmSmartFigurePage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmSmartFigurePage : Page
    {
        public AlarmSmartFigurePage()
        {
            InitializeComponent();
            this.DataContext = new AlarmSmartFigureViewModel();
            this.t4pager.SelectionChangedEvent += Pager4_SelectionChangedEvent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    string tag = this.t4robotTag.Text.Trim();
                    string sTime = this.t4startTime.UDateTime;
                    string eTime = this.t4endTime.UDateTime;
                    (this.DataContext as AlarmSmartFigureViewModel).RefreshDataGrid(tag, sTime, eTime);
                }));
            });
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager4_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as AlarmSmartFigureViewModel).ChangePageSize(pageSize);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T4SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.t4robotTag.Text.Trim();
            string sTime = this.t4startTime.UDateTime;
            string eTime = this.t4endTime.UDateTime;
            (this.DataContext as AlarmSmartFigureViewModel).RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T4DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除智能判图报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = t4dataGrid.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (t4dataGrid.SelectedItems[i] as SmartFigureAlarm).Id;
                }

                AlarmmanagerDal dal = new AlarmmanagerDal();
                bool flag = dal.DeleteSmartFigureAlarm(ids);
                if (flag)
                {
                    (this.DataContext as AlarmSmartFigureViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除智能判图报警失败!").ShowDialog();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T4SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t4dataGrid.Items.Count; i++)
            {
                var cntr = t4dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T4SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t4dataGrid.Items.Count; i++)
            {
                var cntr = t4dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }

        /// <summary>
        /// 查看智能判图现场图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T4xct_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as SmartFigureAlarm;
            new ShowImgFlyout(obj.AlarmImgUrl).ShowDialog();
        }
    }
}
