
using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace MaxRobotServerApp.Views.Pages
{
    /// <summary>
    /// ResourcesAppPage.xaml 的交互逻辑
    /// </summary>
    public partial class ResourcesAppPage : Page
    {
        public ResourcesAppPage()
        {
            InitializeComponent();
            this.DataContext = new AppsourcesViewModel();
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
            (this.grid2.DataContext as AppsourcesViewModel).PageSize = pageSize;
            (this.grid2.DataContext as AppsourcesViewModel).RefreshDataGrid();
        }

        /// <summary>
        /// 添加宣讲资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AppresourcesFlyout newform = new AppresourcesFlyout(null);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.grid2.DataContext as AppsourcesViewModel).RefreshDataGrid();
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2Edit_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as Appresource;
            AppresourcesFlyout newform = new AppresourcesFlyout(obj);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.grid2.DataContext as AppsourcesViewModel).RefreshDataGrid();
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除App资源");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = dataGrid2.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (dataGrid2.SelectedItems[i] as Appresource).Id;
                }

                AppresourceDal dal = new AppresourceDal();
                bool flag = dal.Delete(ids);
                if (flag)
                {
                    (this.grid2.DataContext as AppsourcesViewModel).RefreshDataGrid();
                }
                else
                {
                    new Warning("删除App资源失败!").ShowDialog();
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
        /// 反选
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

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2Upload_Click(object sender, RoutedEventArgs e)
        {
            #region 选择文件夹
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            var res = dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK;
            if (!res) return;
            var dir = dlg.SelectedPath;
            #endregion

            DownloadFlyout.GetInstance().Show();
            try
            {
                var obj = ((Image)sender).DataContext as Appresource;
                CancellationTokenSource cts = new CancellationTokenSource();
                FileDownload download = new FileDownload();
                string savepath = $@"{dir}\{Path.GetFileName(obj.AppFileUrl)}";
                do
                {
                    if (File.Exists(savepath))
                    {
                        savepath = $@"{Path.GetDirectoryName(savepath)}\副本_{Path.GetFileName(savepath)}";
                    }
                }
                while (File.Exists(savepath));
                string id = Guid.NewGuid().ToString("N");   //下载文件的guid
                DownloadFlyout.GetInstance().AddDownloadInfo(new DownloadInfo()
                {
                    Id = id,
                    Name = Path.GetFileName(savepath),
                    ProgressSize = 0,
                    Percentage = (0 / 100.0).ToString("P2"),
                    State = "正在下载",
                    Cts = cts
                });
                download.DownloadAsync(savepath, obj.AppFileUrl, (p) =>
                {
                    Console.WriteLine("下载进度:" + p);
                    DownloadFlyout.GetInstance().ProgressSizeChanged(id, p);
                }, (c) =>
                {
                    if (c)
                    {
                        Console.WriteLine("下载成功");
                        DownloadFlyout.GetInstance().DownloadStateChanged(id, "下载成功");
                    }
                    else
                    {
                        Console.WriteLine("下载失败");
                        DownloadFlyout.GetInstance().DownloadStateChanged(id, "下载失败");
                    }
                }, (error) =>
                {
                    Console.WriteLine(error);
                }, cts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
