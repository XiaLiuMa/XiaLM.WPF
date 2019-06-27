using MyCustomControl;

using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// MapInfoFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class MapInfoFlyout : CBaseFlyout
    {
        private event Action<string> mapidTextChange;
        private event Action<string> ennameTextChange;
        private MapResourceDal dal;
        public MapInfoFlyout()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            dal = new MapResourceDal();
            DataObject.AddPastingHandler(this.enname, Enname_PastingEvent);
            DataObject.AddPastingHandler(this.az, az_PastingEvent);
            DataObject.AddPastingHandler(this.txtMapHeight, az_PastingEvent);
            DataObject.AddPastingHandler(this.txtMapWidth, az_PastingEvent);
            var obs_mapid = Observable.FromEvent<string>(p => this.mapidTextChange += p, p => this.mapidTextChange -= p);
            var obs_enname = Observable.FromEvent<string>(p => this.ennameTextChange += p, p => this.ennameTextChange -= p);
            obs_mapid.Subscribe((s) =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        this.mapid_tip.Content = "";
                    }
                    else
                    {
                        if (dal.ExitsMapId(s))
                        {
                            this.mapid_tip.Content = "地图编号已存在";
                        }
                        else
                        {
                            this.mapid_tip.Content = "";
                        }
                    }
                }));
            });
            obs_enname.Subscribe((s) =>
            {
                this.Dispatcher?.Invoke(new Action(() =>
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        this.enname_tip.Content = "";
                    }
                    else
                    {
                        if (dal.ExitsEnName(s))
                        {
                            this.enname_tip.Content = "地图英文名称已存在";
                        }
                        else
                        {
                            this.enname_tip.Content = "";
                        }
                    }
                }));
            });
        }
        private MapResource _model;
        public MapInfoFlyout(MapResource model) : this()
        {
            _model = model;
            this.txtMapWidth.Text = "0";
            this.txtMapHeight.Text = "0";
            this.mapid.Text = model.MapId;
            this.mapid.IsEnabled = false;
            this.name.Text = model.Name;
            this.enname.Text = model.EnName;
            this.enname.IsEnabled = false;
            this.minZoom.Value = model.MinZoom;
            this.maxZoom.Value = model.MaxZoom;
            this.zoom.Value = model.Zoom;
            this.az.Text = model.Az.ToString();
        }
        private void az_PastingEvent(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                //var pastingText = (String)e.DataObject.GetData(typeof(String));
                //var re = new Regex("[^0-9.-]+");
                //e.Handled = re.IsMatch(pastingText);
                //if (e.Handled)
                //{
                e.CancelCommand();
                //}
            }
        }

        private void Enname_PastingEvent(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var pastingText = (String)e.DataObject.GetData(typeof(String));
                var re = new Regex("[^QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0-9_]+");
                e.Handled = re.IsMatch(pastingText);
                if (e.Handled)
                {
                    e.CancelCommand();
                }
            }
        }



        /// <summary>
        /// 提交成功事件
        /// </summary>
        public event Action ConfirmSuccess = () => { };

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.selectpath.Path;
            var mapidText = this.mapid.Text;
            var nameText = this.name.Text;
            var ennameText = this.enname.Text;
            var minzoomText = (int)this.minZoom.Value;
            var maxzoomText = (int)this.maxZoom.Value;
            var zoomText = (int)this.zoom.Value;
            var azText = this.az.Text;
            if (!File.Exists(filename))
            {
                this.selectpath_tip.Content = "文件路径错误";
                return;
            }
            if (string.IsNullOrEmpty(mapidText))
            {
                this.mapid_tip.Content = "地图编号不能为空";
                return;
            }
            else
            {
                if(_model==null)
                if (dal.ExitsMapId(mapidText))
                {
                    this.mapid_tip.Content = "地图编号已存在";
                    return;
                }
            }
            if (string.IsNullOrEmpty(nameText))
            {
                this.name_tip.Content = "名称不能为空";
                return;
            }
            if (string.IsNullOrEmpty(ennameText))
            {
                this.enname_tip.Content = "地图编号不能为空";
                return;
            }
            else
            {
                if (_model == null)
                    if (dal.ExitsEnName(ennameText))
                {
                    this.enname_tip.Content = "英文名称已存在";
                    return;
                }
            }
            if (string.IsNullOrEmpty(azText))
            {
                this.az.Text = "0";
            }
            var width = int.Parse(string.IsNullOrEmpty(this.txtMapWidth.Text) ? "0" : this.txtMapWidth.Text);
            var height = int.Parse(string.IsNullOrEmpty(this.txtMapHeight.Text) ? "0" : this.txtMapHeight.Text);
            var upload = new FileUpload(filename);
            upload.Upload().Completed((b, s) =>
            {
                if (b)
                {
                    var mod = new Maxvision.Robot.Sdk.Model.MapInfoParms()
                    {
                        Az = float.Parse(azText),
                        EnName = ennameText,
                        Height = height,
                        MapId = mapidText,
                        MaxZoom = maxzoomText,
                        MinZoom = minzoomText,
                        Name = nameText,
                        Url_FileInfoId = s.FileInfoId,
                        Width = width,
                        Zoom = zoomText
                    };
                    var b1 = false;
                    if (_model == null)
                        b1 = dal.AddMapInfo(mod);
                    else
                    {
                        b1 = dal.UpdataMapInfo(mod);
                    }
                    if (b1)
                    {
                        ConfirmSuccess?.Invoke();
                    }
                    else
                    {
                        this.Dispatcher?.Invoke(() =>
                        {
                            MessageBox.Show("提交失败");
                        });
                    }
                }
                else
                {
                    this.Dispatcher?.Invoke(() =>
                    {
                        MessageBox.Show("提交失败");
                    });
                }
            }).Progress((s) =>
            {
                this.Dispatcher?.Invoke(() =>
                {
                    this.ProgressBar.Value = s;
                });
            });
        }


        private void Zoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.zoom_tip.Content = $"等级:{(int)e.NewValue}";
        }

        private void MaxZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.maxZoom_tip.Content = $"等级:{(int)e.NewValue}";
        }

        private void MinZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.minZoom_tip.Content = $"等级:{(int)e.NewValue}";
        }

        private void Az_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Enname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0-9_]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Enname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ennameTextChange?.Invoke(this.enname.Text);
        }

        private void Mapid_TextChanged(object sender, TextChangedEventArgs e)
        {
            mapidTextChange?.Invoke(this.mapid.Text);
        }

        private void Selectpath_OnSelected(string filename)
        {
            var width = 0;
            var height = 0;
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                System.Drawing.Image image = null;
                try
                {
                    image = System.Drawing.Image.FromStream(fs);
                    width = image.Width;
                    height = image.Height;
                }
                catch (Exception)
                {
                }
                image?.Dispose();
            }
            this.txtMapWidth.Text = width.ToString();
            this.txtMapHeight.Text = height.ToString();
        }

        private void TxtMapHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void TxtMapWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
