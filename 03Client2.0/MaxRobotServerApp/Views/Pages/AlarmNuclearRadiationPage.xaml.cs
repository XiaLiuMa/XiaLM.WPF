
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
    /// AlarmNuclearRadiationPage.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmNuclearRadiationPage : Page
    {
        public AlarmNuclearRadiationPage()
        {
            InitializeComponent();
            this.DataContext = new AlarmNuclearRadiationViewModel();
            this.t5pager.SelectionChangedEvent += Pager5_SelectionChangedEvent;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    string tag = this.t5robotTag.Text.Trim();
                    string sTime = this.t5startTime.UDateTime;
                    string eTime = this.t5endTime.UDateTime;
                    (this.DataContext as AlarmNuclearRadiationViewModel).RefreshDataGrid(tag, sTime, eTime);
                }));
            });
        }

        /// <summary>
        /// 行数切换事件
        /// </summary>
        /// <param name="pageSize"></param>
        private void Pager5_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as AlarmNuclearRadiationViewModel).ChangePageSize(pageSize);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T5SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.t5robotTag.Text.Trim();
            string sTime = this.t5startTime.UDateTime;
            string eTime = this.t5endTime.UDateTime;
            (this.DataContext as AlarmNuclearRadiationViewModel).RefreshDataGrid(tag, sTime, eTime);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T5DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除核辐射报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = t5dataGrid.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (t5dataGrid.SelectedItems[i] as NuclearRadiationAlarm).Id;
                }

                AlarmmanagerDal dal = new AlarmmanagerDal();
                bool flag = dal.DeleteNuclearRadiationAlarm(ids);
                if (flag)
                {
                    (this.DataContext as AlarmNuclearRadiationViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除核辐射报警失败!").ShowDialog();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T5SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t5dataGrid.Items.Count; i++)
            {
                var cntr = t5dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T5SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.t5dataGrid.Items.Count; i++)
            {
                var cntr = t5dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }
    }
}
