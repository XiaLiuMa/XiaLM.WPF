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
using System.Windows.Shapes;

namespace XiaLM.WPF.T01.App.Windows
{
    /// <summary>
    /// UpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateButton.IsEnabled = false;

            //try
            //{
            //    WebRequest request = WebRequest.Create(NewDownloadUrl);
            //    WebResponse respone = request.GetResponse();
            //    PbProgress.Maximum = respone.ContentLength;
            //    if (!Directory.Exists(ThisPath + TmpPath))
            //    {
            //        Directory.CreateDirectory(ThisPath + TmpPath);
            //    }
            //    ThreadPool.QueueUserWorkItem((obj) =>
            //    {
            //        Stream netStream = respone.GetResponseStream();
            //        Stream fileStream = new FileStream(ThisPath + TmpPath + "app.zip", FileMode.Create);
            //        byte[] read = new byte[1024];
            //        long progressBarValue = 0;
            //        int realReadLen = netStream.Read(read, 0, read.Length);
            //        while (realReadLen > 0)
            //        {
            //            fileStream.Write(read, 0, realReadLen);
            //            progressBarValue += realReadLen;
            //            PbProgress.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
            //            realReadLen = netStream.Read(read, 0, read.Length);
            //        }
            //        netStream.Close();
            //        fileStream.Close();
            //        Mainthread.BeginInvoke(new Action(() =>
            //        {
            //            //BtOk.IsEnabled = true;

            //            //BtOk.Content = "安装新版";
            //            DetailText.AppendText($@"文件app.zip下载完成！" + "\r\n 准备安装……");
            //            ReplaceOperation();

            //        }));
            //    }, null);
            //}
            //catch (Exception ex)
            //{
            //    Mainthread.BeginInvoke(new Action(() =>
            //    {
            //        DetailText.AppendText("更新失败！\r\n" + ex.Message + "\r\n" + ex.InnerException);
            //    }));
            //}
        }
    }
}
