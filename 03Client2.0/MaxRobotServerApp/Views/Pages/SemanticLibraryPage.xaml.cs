
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
    /// SemanticLibraryPage.xaml 的交互逻辑
    /// </summary>
    public partial class SemanticLibraryPage : Page
    {
        public SemanticLibraryPage()
        {
            InitializeComponent();
            this.DataContext = new SemanticLibraryViewModel();
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
            (this.DataContext as SemanticLibraryViewModel).PageSize = pageSize;
            (this.DataContext as SemanticLibraryViewModel).RefreshDataGrid();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1AddBtn_Click(object sender, RoutedEventArgs e)
        {
            SemanticlibrarymanagerFlyout newform = new SemanticlibrarymanagerFlyout(null);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as SemanticLibraryViewModel).RefreshDataGrid();
                    (this.DataContext as SemanticViewModel).RefreshComBox();
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1AlterBtn_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as SemanticGenreInfo;
            SemanticlibrarymanagerFlyout newform = new SemanticlibrarymanagerFlyout(obj);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as SemanticLibraryViewModel).RefreshDataGrid();
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除语义库");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = this.dataGrid1.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (this.dataGrid1.SelectedItems[i] as SemanticGenreInfo).Id;
                }

                SemanticmanagerDal dal = new SemanticmanagerDal();
                bool flag = dal.DeleteSemanticGenre(ids);
                if (flag)
                {
                    (this.DataContext as SemanticLibraryViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除语义库失败!").ShowDialog();
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
        /// 全不选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1SelectInvert_Click(object sender, RoutedEventArgs e)
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
