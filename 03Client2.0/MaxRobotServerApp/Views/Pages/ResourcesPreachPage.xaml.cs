
using MaxRobotServerApp.Views.ViewModels;
using System;
using System.IO;
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
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Dals;
using System.Threading;
using Maxvision.Robot.Sdk;

namespace MaxRobotServerApp.Views.Pages
{
    /// <summary>
    /// ResourcesPreachPage.xaml 的交互逻辑
    /// </summary>
    public partial class ResourcesPreachPage : Page
    {
        public ResourcesPreachPage()
        {
            InitializeComponent();
            this.DataContext = new PreachresourcesViewModel();
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
            (this.DataContext as PreachresourcesViewModel).PageSize = pageSize;
            (this.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1SelectBtu_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
        }

        /// <summary>
        /// 添加宣讲资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1AddBtn_Click(object sender, RoutedEventArgs e)
        {
            PreachresourcesFlyout newform = new PreachresourcesFlyout(null);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 编辑资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1Edit_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as Preachresource;
            PreachresourcesFlyout newform = new PreachresourcesFlyout(obj);
            newform.ConfirmSuccess += () =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
                }));
            };
            newform.ShowDialog();
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除宣讲资源");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                int count = dataGrid1.SelectedItems.Count;
                if (count <= 0) return;
                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = (dataGrid1.SelectedItems[i] as Preachresource).Id;
                }
                var taskdal = new TaskMangerDal();
                var p = 0;
                foreach (var item in ids)
                {
                    var pre = (dataGrid1.SelectedItems[p] as Preachresource);
                    var b = taskdal.IsResourceUsed(pre.Id);
                    if (b)
                    {
                        MessageBox.Show($"资源({pre.Name})已在任务中配置，如需删除请先解除关联！");
                        return;
                    }
                    p++;
                }
                PreachresourceDal preachresourceDal = new PreachresourceDal();
                bool flag = preachresourceDal.Delete(ids);
                if (flag)
                {
                    (this.DataContext as PreachresourcesViewModel).RefreshDataGrid(this.filetypeComBox.SelectedIndex);
                }
                else
                {
                    new Warning("删除宣讲资源失败!").ShowDialog();
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
        /// 反选
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

        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1tp_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Image)sender).DataContext as Preachresource;
            new ShowImgFlyout(obj.ThumbnailFileInfoPath).ShowDialog();
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T1Upload_Click(object sender, RoutedEventArgs e)
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
                var obj = ((Image)sender).DataContext as Preachresource;
                CancellationTokenSource cts = new CancellationTokenSource();
                FileDownload download = new FileDownload();
                string savepath = $@"{dir}\{Path.GetFileName(obj.FileInfoPath)}";
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
                    Download = download,
                    Cts = cts
                });

                download.DownloadAsync(savepath, obj.FileInfoPath, (p) =>
                {
                    Console.WriteLine("下载进度:" + p);
                    this.Dispatcher?.Invoke(new Action(() =>
                    {
                        DownloadFlyout.GetInstance().ProgressSizeChanged(id, p);
                    }));

                }, (c) =>
                {
                    if (c)
                    {
                        Console.WriteLine("下载成功");
                        this.Dispatcher?.Invoke(new Action(() =>
                        {
                            DownloadFlyout.GetInstance().DownloadStateChanged(id, "下载成功");
                        }));

                    }
                    else
                    {
                        Console.WriteLine("下载失败");
                        this.Dispatcher?.Invoke(new Action(() =>
                        {
                            DownloadFlyout.GetInstance().DownloadStateChanged(id, "下载失败");
                        }));

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
