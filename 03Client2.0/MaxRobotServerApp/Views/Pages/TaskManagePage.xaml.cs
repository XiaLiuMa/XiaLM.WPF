
using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
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
    /// TaskManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class TaskManagePage : Page
    {
        public Action Refresh;
        public TaskManagePage()
        {
            InitializeComponent();
            var task = new TaskManagerViewModel();
            DataContext = task;
            this.pager1.SelectionChangedEvent += Pager1_SelectionChangedEvent;
            Refresh = () =>
            {
                (this.DataContext as TaskManagerViewModel).RefreshDataGrid("");
            };
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

        private void Pager1_SelectionChangedEvent(int obj)
        {
            (this.DataContext as TaskManagerViewModel).PageSize = obj;
            (this.DataContext as TaskManagerViewModel).RefreshDataGrid("");
        }

        private void TbControl_KeyDown(object sender, KeyEventArgs e)
        {

        }
        //查询
        private void QueryBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as TaskManagerViewModel).RefreshDataGrid(this.txtQuery.Text);
        }
        //添加
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var flyoutSelect = new TaskTypeSelectFlyout(Refresh)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            flyoutSelect.ShowDialog();
        }
        //删除
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            int count = dataGrid1.SelectedItems.Count;
            if (count <= 0) return;
            int[] ids = new int[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = (dataGrid1.SelectedItems[i] as TaskInfoMod).Id;
            }

            var dal = new TaskMangerDal();
            bool flag = dal.DeleteTask(ids);
            if (flag)
            {
                (this.DataContext as TaskManagerViewModel).RefreshDataGrid("");
            }
        }
        //编辑
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var mod = ((Image)sender).DataContext as TaskInfoMod;
            if (mod != null)
            {
                switch (mod.TaskType)
                {
                    case TaskType.cruise:
                        {
                            var flyout = new TaskCruiseFlyout(Refresh, mod);
                            flyout.ShowDialog();
                            break;
                        }
                    case TaskType.line:
                        {
                            var flyout = new TaskLineFlyout(Refresh, mod);
                            flyout.ShowDialog();
                            break;
                        }
                    case TaskType.point:
                        {
                            var flyout = new TaskPositionFlyout(Refresh, mod);
                            flyout.ShowDialog();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        //全选
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
        //全不选
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
    }
}
