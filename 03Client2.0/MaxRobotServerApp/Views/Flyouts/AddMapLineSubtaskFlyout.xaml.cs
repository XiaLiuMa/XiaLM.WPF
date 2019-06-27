using MyCustomControl;

using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// SelectMapLineSubtaskFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class AddMapLineSubtaskFlyout : CBaseFlyout
    {
        public event Action<SubTaskMod> OnAdded;
        public event Action OnError;
        public AddMapLineSubtaskFlyout()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new TaskConfigurationViewModel();
            DataObject.AddPastingHandler(this.txtAngle, PastingEvent);
            DataObject.AddPastingHandler(this.txtTime, PastingEvent);
        }
        private void PastingEvent(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                e.CancelCommand();
            }
        }

        private MapResource _map;
        private bool _isEdit = false;
        public AddMapLineSubtaskFlyout(MapResource map, bool isEdit = false, SubTaskMod subTask = null) : this()
        {
            _map = map;
            _isEdit = isEdit;
            if (_isEdit)
            {
                if (subTask.CmdType == Maxvision.Robot.Sdk.Model.CmdType.function)
                {
                    this.cbTypeSelect.SelectedIndex = 1;
                }
                else { this.cbTypeSelect.SelectedIndex = 0; }
                var mapDal = new MapResourceDal();
                var line = mapDal.QueryLine(subTask.LineId);
                if (line == null)
                {
                    OnError?.Invoke();
                    return;
                }
                subTask.LineId = line.Id;
                var maplineMod = new MapLineMod()
                {
                    Id = line.Id,
                    EnName = line.EnName,
                    LineId = line.LineId,
                    MapId = line.MapId,
                    Name = line.Name
                };
                this.txtLine.Text = $"{maplineMod.Name}({maplineMod.EnName})";
                this.txtLine.DataContext = maplineMod;
                var recDal = new PreachresourceDal();
                var rec = recDal.Query(subTask.PlayerId);
                if (rec != null)
                {
                    var recMod = new Preachresource()
                    {
                        Desc = rec.Desc,
                        FileInfoPath = rec.FileInfoPath,
                        FileSize = rec.FileSize,
                        Id = rec.Id,
                        Name = rec.Name,
                        ResourceType = rec.ResourceType == Maxvision.Robot.Sdk.Model.ResourceType.Video ? "视频" : rec.ResourceType == Maxvision.Robot.Sdk.Model.ResourceType.Voice ? "音频" : "其他",
                        ThumbnailFileInfoPath = rec.ThumbnailFileInfoPath,
                        Time = rec.Time
                    };
                    this.txtPlayer.Text = $"{recMod.Name}({ recMod.ResourceType})";
                    this.txtPlayer.DataContext = recMod;
                }

                this.cbCmdSelect.SelectedIndex = (this.cbCmdSelect.DataContext as TaskConfigurationViewModel).Cmds.Where(r => r.Id.Equals(subTask.Cmd.Split(' ')[0])).First().Num;
                this.cbFunSelect.SelectedIndex = (this.cbFunSelect.DataContext as TaskConfigurationViewModel).Functions.Where(r => r.Id.Equals(subTask.Cmd.Split(' ')[1])).First().Num;
                this.txtAngle.Text = subTask.Angle.ToString();
                this.txtTime.Text = subTask.Duration.ToString();
            }
        }
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            var b = false;
            var item = this.cbTypeSelect.SelectedItem as ComboBoxItem;
            if ((string)item.Content == "执行功能")
            {
                b = true;
            }
            else
            {
                b = false;
            }
            var angle = float.Parse(string.IsNullOrEmpty(this.txtAngle.Text) ? "0" : this.txtAngle.Text);
            var cmd = this.cbCmdSelect.SelectedItem as CmdMod;
            var line = this.txtLine.DataContext as MapLineMod;
            var fun = this.cbFunSelect.SelectedItem as CmdMod;
            var rec = this.txtPlayer.DataContext as Preachresource;
            var duration = int.Parse(string.IsNullOrEmpty(this.txtTime.Text) ? "0" : this.txtTime.Text);
            if (line == null)
            {
                MessageBox.Show("请选择线路");
                return;
            }
            if (b)
            {
                if (fun == null)
                {
                    MessageBox.Show("请选择功能");
                    return;
                }
                else
                {
                    if (fun.Id == "无")
                    {
                        MessageBox.Show("请选择功能");
                        return;
                    }
                }
            }
            else
            {
                if (rec == null)
                {
                    MessageBox.Show("请选择播放资源");
                    return;
                }
            }
            OnAdded?.Invoke(new SubTaskMod()
            {
                Angle = angle,
                CmdStr = cmd.Name + " " + fun.Name,
                Cmd = cmd.Id + " " + fun.Id,
                CmdType = b ? Maxvision.Robot.Sdk.Model.CmdType.function : Maxvision.Robot.Sdk.Model.CmdType.player,
                CmdTypeStr = b ? "执行功能" : "播放媒体",
                Duration = duration,
                LineId = line.Id,
                Name = line.Name + $"({line.EnName})",
                Number = 0,
                PlayerId = rec == null ? -1 : rec.Id,
                PositionId = -1,
                RecName = rec == null ? "" :b?"": rec.Name
            });
            this.Close();
        }

        private void BtnSelectLine_Click(object sender, RoutedEventArgs e)
        {
            var mapid = _map;
            if (mapid == null)
            {
                MessageBox.Show("请先选择地图");
                return;
            }
            var flyout = new SelectMapLineFlyout(mapid.MapId, Maxvision.Robot.Sdk.Model.LineType.major);
            flyout.OnSelected += (s) =>
            {
                this.txtLine.Text = $"{s.Name}({s.EnName})";
                this.txtLine.DataContext = s;
            };
            flyout.ShowDialog();
        }

        private void BtnSelectPlay_Click(object sender, RoutedEventArgs e)
        {
            var flyout = new SelectResourcesFlyout();
            flyout.OnSelected += s =>
            {
                this.txtPlayer.Text = $"{s.Name}({ s.ResourceType})";
                this.txtPlayer.DataContext = s;
            };
            flyout.ShowDialog();
        }

        private void TxtAngle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9.-]");
            e.Handled = re.IsMatch(e.Text);
        }

        private void CbTypeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((ComboBox)sender).SelectedItem as ComboBoxItem;
            if (item == null) return;
            if ((string)item.Content == "执行功能")
            {
                this.btnSelectPlay.IsEnabled = false;
                this.txtPlayer.IsEnabled = false;
                this.cbFunSelect.IsEnabled = true;
            }
            else
            {
                this.cbFunSelect.IsEnabled = false;
                this.btnSelectPlay.IsEnabled = true;
                this.txtPlayer.IsEnabled = true;
            }
        }

        private void TxtTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
