
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using MyCustomControl;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// PreachresourcesFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadFlyout : CBaseFlyout
    {
        #region 单例
        private static DownloadFlyout instance;
        private readonly static object objLock = new object();

        public static DownloadFlyout GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new DownloadFlyout();
                    }
                }
            }
            return instance;
        }
        #endregion

        public DownloadFlyout()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInit()
        {
            this.DataContext = new DownloadFlyoutViewModel();
        }

        public void AddDownloadInfo(DownloadInfo item)
        {
            try
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as DownloadFlyoutViewModel).List1AddItem(item);
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 下载进度值变化
        /// </summary>
        /// <param name="id"></param>
        /// <param name="progresssize"></param>
        public void ProgressSizeChanged(string id, double progresssize)
        {
            try
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as DownloadFlyoutViewModel).ProgressSizeChanged(id, progresssize);
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 下载状态变化事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void DownloadStateChanged(string id, string state)
        {
            try
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    (this.DataContext as DownloadFlyoutViewModel).DownloadStateChanged(id, state);
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 暂停下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            var btu = sender as Button;
            if (btu == null) return;
            var obj = ((Button)sender).DataContext as DownloadInfo;
            if (btu.Content.Equals("暂停"))
            {
                obj.Download.Pause(true);   //暂停下载
                (sender as Button).Content = "开始";
            }
            else if (btu.Content.Equals("开始"))
            {
                obj.Download.Pause(false);   //开始下载
                (sender as Button).Content = "暂停";
            }
        }


        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            RemindFlyout remind = new RemindFlyout("正在删除核辐射报警");
            remind.ShowDialog();
            if (remind.IsConfirm)
            {
                var obj = ((Button)sender).DataContext as DownloadInfo;
                obj.Download.Dispose();   //取消下载

                (this.DataContext as DownloadFlyoutViewModel).List1DeleteItem(new string[] { obj.Id });
            }
        }

        /// <summary>
        /// 删除下载完成单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2Delete_Click(object sender, RoutedEventArgs e)
        {
            var obj = ((Button)sender).DataContext as DownloadInfo;
            (this.DataContext as DownloadFlyoutViewModel).List2DeleteItem(new string[] { obj.Id });
        }

        /// <summary>
        /// 删除下载完成多项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2DeleteList_Click(object sender, RoutedEventArgs e)
        {
            int count = dataGrid2.SelectedItems.Count;
            if (count <= 0) return;
            string[] ids = new string[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = (dataGrid2.SelectedItems[i] as DownloadInfo).Id;
            }
            (this.DataContext as DownloadFlyoutViewModel).List2DeleteItem(ids);
        }

        /// <summary>
        /// 清空下载完成所有项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void T2Clear_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as DownloadFlyoutViewModel).List2ClearItem();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadFlyout_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();    //隐藏当前弹框
        }
    }
}
