using MyCustomControl;

using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Extensions.Comm;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// AppresourcesFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class AppresourcesFlyout : CBaseFlyout
    {
        /// <summary>
        /// 编辑用的数据Id
        /// </summary>
        private int Id { get; set; } = 0;
        /// <summary>
        /// 是否是编辑模式，否则是添加模式
        /// </summary>
        private bool IsEdit { get; set; }

        /// <summary>
        /// 更新类型类型
        /// </summary>
        private UpdateMode updateMode
        {
            get
            {
                UpdateMode mode = UpdateMode.Optional;
                switch (this.updatetypeComBox.SelectedIndex)
                {
                    case 1:
                        mode = UpdateMode.Force;
                        break;
                    default:
                        mode = UpdateMode.Optional;
                        break;
                }
                return mode;
            }
        }

        /// <summary>
        /// 提交成功事件
        /// </summary>
        public event Action ConfirmSuccess = () => { };

        public AppresourcesFlyout(Appresource obj)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit(obj);
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInit(Appresource obj)
        {
            if (obj != null)
            {
                IsEdit = true;
                Id = obj.Id;
                this.appname.Text = obj.Name;
                this.appversion.Text = obj.Version;
                this.appdesc.Text = obj.Desc;
                switch (obj.UpdateMode)
                {
                    case "强制":
                        this.updatetypeComBox.SelectedIndex = 1;
                        break;
                    default:
                        this.updatetypeComBox.SelectedIndex = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(this.selectpath.Path.Trim())) return;  //未填写文件路径
            try
            {
                string path = this.selectpath.Path.Trim();
                AppParms parms = new AppParms()
                {
                    Name = this.appname.Text.Trim(),
                    Version = this.appversion.Text.Trim(),
                    Desc = this.appdesc.Text.Trim(),
                    UpdateMode = updateMode
                };

                Dictionary<string, FileUpload> dic = new Dictionary<string, FileUpload>();
                dic.Add("app", new FileUpload(path));
                FileUploadGroup uploadGroup = new FileUploadGroup(dic);
                uploadGroup.Upload().Completed((s) =>
                {
                    var f1 = s.Where(r => r.Key.Equals("app")).FirstOrDefault();
                    if (f1.Value.Completed)
                    {
                        parms.Url_FileInfo_Id = f1.Value.FileInfoId;
                        AppresourceDal dal = new AppresourceDal();
                        if (IsEdit)
                        {
                            parms.Id = Id;
                            bool flag = dal.Updata(parms);
                            if (flag)
                            {
                                ConfirmSuccess();
                            }
                            else
                            {
                                new Warning("编辑App资源失败!").ShowDialog();
                            }
                        }
                        else
                        {
                            bool flag = dal.Add(parms);
                            if (flag)
                            {
                                ConfirmSuccess();
                            }
                            else
                            {
                                new Warning("添加App资源失败!").ShowDialog();
                            }
                        }
                        //关闭窗体
                        this.Dispatcher?.Invoke(new Action(() =>
                        {
                            this.Close();
                        }));
                    }
                    else
                    {
                        //Console.WriteLine($"key:{item.Key},上传失败");
                    }
                }).Progress((s) =>
                {
                    this.Dispatcher?.Invoke(new Action(() =>
                    {
                        if (AnimateBehavior.IsAnimating) return;
                        ProgressBar.Value = Math.Min(ProgressBar.Maximum, s.Progress);
                    }));

                    Console.WriteLine($"多文件进度：key：{s.Key}进度：" + s.Progress);
                }).Exception((s) => Console.WriteLine(s));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
