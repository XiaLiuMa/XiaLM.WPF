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
    /// AddMapPositionSubtaskFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class AddMapPositionSubtaskFlyout : CBaseFlyout
    {
        public event Action<SubTaskMod> OnAdded;
        public event Action OnError;
        public AddMapPositionSubtaskFlyout()
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
        public AddMapPositionSubtaskFlyout(MapResource map, bool isEdit = false, SubTaskMod subTask = null) : this()
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
                var position = mapDal.QueryPosition(subTask.PositionId);
                if (position == null)
                {
                    OnError?.Invoke();
                    return;
                }
                subTask.PositionId = position.Id;
                var mappositionMod = new MapPositionMod()
                {
                    Id = position.Id,
                    EnName = position.EnName,
                    Mapid = position.Mapid,
                    Name = position.Name,
                    PosId = position.PosId
                };
                this.txtPosition.Text = $"{mappositionMod.Name}({mappositionMod.EnName})";
                this.txtPosition.DataContext = mappositionMod;
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
            var position = this.txtPosition.DataContext as MapPositionMod;
            var fun = this.cbFunSelect.SelectedItem as CmdMod;
            var rec = this.txtPlayer.DataContext as Preachresource;
            var duration = int.Parse(string.IsNullOrEmpty(this.txtTime.Text) ? "0" : this.txtTime.Text);
            if (position == null)
            {
                MessageBox.Show("请选择坐标点");
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
                LineId = -1,
                Name = position.Name + $"({position.EnName})",
                Number = 0,
                PlayerId = rec == null ? -1 : rec.Id,
                PositionId = position.Id,
                RecName = rec == null ? "" : b ? "" : rec.Name
            });
            this.Close();
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

        private void BtnSelectPosition_Click(object sender, RoutedEventArgs e)
        {
            var mapid = _map;
            if (mapid == null)
            {
                MessageBox.Show("请先选择地图");
                return;
            }
            var flyout = new SelectMapPositionFlyout(mapid.MapId);
            flyout.OnSelected += (s) =>
            {
                this.txtPosition.Text = $"{s.Name}({s.EnName})";
                this.txtPosition.DataContext = s;
            };
            flyout.ShowDialog();
        }
    }
}
