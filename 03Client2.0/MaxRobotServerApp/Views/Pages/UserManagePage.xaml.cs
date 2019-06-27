
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
    /// UserManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagePage : Page
    {
        public UserManagePage()
        {
            InitializeComponent();
            this.DataContext = new UserconfigViewModel();
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
            (this.DataContext as UserconfigViewModel).PageSize = pageSize;
            (this.DataContext as UserconfigViewModel).RefreshDataGrid();
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            UserconfigFlyout newform = new UserconfigFlyout(null);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as UserconfigViewModel).RefreshDataGrid();
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as Userinfo;
            UserconfigFlyout newform = new UserconfigFlyout(obj);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as UserconfigViewModel).RefreshDataGrid();
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除用户");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = dataGrid1.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (dataGrid1.SelectedItems[i] as Userinfo).Id;
                }

                UserManageDal dal = new UserManageDal();
                bool flag = dal.Delete(ids);
                if (flag)
                {
                    (this.DataContext as UserconfigViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除用户失败!").ShowDialog();
                }
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
    }
}
