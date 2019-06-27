using MyCustomControl;

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
using MaxRobotServerApp.Extensions.Utils;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// PreachresourcesFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class PreachresourcesFlyout : CBaseFlyout
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
        /// 资源类型
        /// </summary>
        private ResourceType resourceType;

        /// <summary>
        /// 提交成功事件
        /// </summary>
        public event Action ConfirmSuccess = () => { };

        public PreachresourcesFlyout(Preachresource obj)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit(obj);
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInit(Preachresource obj)
        {
            if (obj != null)
            {
                IsEdit = true;
                Id = obj.Id;
                switch (obj.ResourceType)
                {
                    case "视频":
                        this.filetypeComBox.SelectedIndex = 0;
                        break;
                    case "音频":
                        this.filetypeComBox.SelectedIndex = 1;
                        break;
                    case "文本":
                        this.filetypeComBox.SelectedIndex = 2;
                        break;
                    case "图片":
                        this.filetypeComBox.SelectedIndex = 3;
                        break;
                    default:
                        this.filetypeComBox.SelectedIndex = -1;
                        break;
                }
                this.filedesc.Text = obj.Desc;
            }
            else
            {
                this.filetypeComBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 文件类型选择变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiletypeComBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.filetypeComBox.SelectedIndex)
            {
                case 0: //视频
                    this.selectpath.Filter = "MP4|*.mp4|3GP|*.3gp|MOV|*.mov";
                    resourceType = ResourceType.Video;
                    break;
                case 1: //音频
                    this.selectpath.Filter = "MP3|*.mp3|WAV|*.wav|MA4|*.ma4";
                    resourceType = ResourceType.Voice;
                    break;
                case 2: //文本
                    this.selectpath.Filter = "TXT|*.txt";
                    resourceType = ResourceType.Text;
                    break;
                case 3: //图片
                    this.selectpath.Filter = "PNG|*.png|GIF|*.gif|JPG|*.jpg";
                    resourceType = ResourceType.Picture;
                    break;
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
                string desc = this.filedesc.Text.Trim();
                string path = this.selectpath.Path.Trim();
                Bitmap bm = WindowsThumbnailProvider.GetThumbnail(path);    //得到缩略图

                Dictionary<string, FileUpload> dic = new Dictionary<string, FileUpload>();
                dic.Add("原图", new FileUpload(path));
                dic.Add("缩略图", new FileUpload(ConvertUtil.Bitmap2Bytes(bm), $"slt_{System.IO.Path.GetFileNameWithoutExtension(path)}.png"));
                FileUploadGroup uploadGroup = new FileUploadGroup(dic);
                uploadGroup.Upload().Completed((s) =>
                {
                    var f1 = s.Where(r => r.Key.Equals("原图")).FirstOrDefault();
                    var f2 = s.Where(r => r.Key.Equals("缩略图")).FirstOrDefault();
                    if (f1.Value.Completed && f2.Value.Completed)
                    {
                        //Console.WriteLine($"key:{item.Key},名称：{item.Value.FileName},编号：{item.Value.FileInfoId}.上传成功");

                        ResourceManagement management = new ResourceManagement();
                        ResourceParms parms = new ResourceParms()
                        {
                            Name = System.IO.Path.GetFileNameWithoutExtension(path),
                            Desc = desc,
                            ResourceType = resourceType,
                            ThumbnailFileInfoId = f2.Value.FileInfoId,
                            FileInfoId = f1.Value.FileInfoId
                        };

                        if (IsEdit)
                        {
                            parms.Id = Id;
                            bool flag = management.Updata(parms);
                            if (flag)
                            {
                                ConfirmSuccess();
                            }
                            else
                            {
                                new Warning("编辑宣讲资源失败!").ShowDialog();
                            }
                        }
                        else
                        {
                            bool flag = management.Add(parms);
                            if (flag)
                            {
                                ConfirmSuccess();
                            }
                            else
                            {
                                new Warning("添加宣讲资源失败!").ShowDialog();
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
