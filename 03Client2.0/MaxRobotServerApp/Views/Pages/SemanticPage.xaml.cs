
using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Extensions;
using MaxRobotServerApp.Extensions.Utils;
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// SemanticPage.xaml 的交互逻辑
    /// </summary>
    public partial class SemanticPage : Page
    {
        public SemanticPage()
        {
            InitializeComponent();
            this.DataContext = new SemanticLibraryViewModel();
            this.pager2.SelectionChangedEvent += Pager2_SelectionChangedEvent;
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
        private void Pager2_SelectionChangedEvent(int pageSize)
        {
            (this.DataContext as SemanticViewModel).PageSize = pageSize;
            (this.DataContext as SemanticViewModel).RefreshDataGrid((this.semanticgenre2.SelectedItem as SemanticGenreInfo).Name);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SemanticViewModel).RefreshDataGrid((this.semanticgenre2.SelectedItem as SemanticGenreInfo).Name);
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = "";
            SelectPathUtil.OpenSelectFileDialog(ref path, "Excel|*.xls;*.xlsx;");
            if (!File.Exists(path)) return;
            List<SemanticInfo> lst = SemanticNpoi.LoadExecl(path, 0, 1);
            SemanticGenreInfo genreInfo = this.semanticgenre2.SelectedItem as SemanticGenreInfo;
            SemanticmanagerDal dal = new SemanticmanagerDal();
            bool flag = dal.AddSemanticInfo(lst, genreInfo);
            if (flag)
            {
                (this.DataContext as SemanticViewModel).RefreshDataGrid((this.semanticgenre2.SelectedItem as SemanticGenreInfo).Name);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除语义");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = this.dataGrid2.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (this.dataGrid2.SelectedItems[i] as SemanticInfo).Id;
                }

                SemanticmanagerDal dal = new SemanticmanagerDal();
                bool flag = dal.DeleteSemanticInfo(ids);
                if (flag)
                {
                    (this.DataContext as SemanticViewModel).RefreshDataGrid((this.semanticgenre2.SelectedItem as SemanticGenreInfo).Name);
                }
                else
                {
                    new Warning("删除语义失败!").ShowDialog();
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
            for (int i = 0; i < this.dataGrid2.Items.Count; i++)
            {
                var cntr = dataGrid2.ItemContainerGenerator.ContainerFromIndex(i);
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
        private void T2SelectInvert_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.dataGrid2.Items.Count; i++)
            {
                var cntr = dataGrid2.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    ObjROw.IsSelected = !ObjROw.IsSelected;
                }
            }
        }
    }
}
