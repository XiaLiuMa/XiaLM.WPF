using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using MyCustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// TaskCruiseFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class TaskCruiseFlyout : CBaseFlyout
    {
        public TaskCruiseFlyout()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new TaskConfigurationViewModel();
            DataObject.AddPastingHandler(this.txtAngle, PastingEvent);
            DataObject.AddPastingHandler(this.txtCirculateCount, PastingEvent);
        }
        private Action Refresh;
        public TaskCruiseFlyout(Action refresh) : this()
        {
            Refresh = refresh;
        }
        private TaskInfoMod _taskInfoMod;
        public TaskCruiseFlyout(Action refresh, TaskInfoMod taskInfoMod) : this()
        {
            Refresh = refresh;
            _taskInfoMod = taskInfoMod;
            this.txtName.Text = taskInfoMod.Name;
            this.txtDesc.Text = taskInfoMod.Desc;
            var tcvm = DataContext as TaskConfigurationViewModel;
            var num = 0;
            foreach (var item in tcvm.MapInfos)
            {
                if (item.MapId.Equals(taskInfoMod.Mapid))
                {
                    break;
                }
                num++;
            }

            this.cbMapSelect.SelectedIndex = num;
            this.cbMapSelect.IsEnabled = false;
            var mapDal = new MapResourceDal();
            var taskDal = new TaskMangerDal();
            var recDal = new PreachresourceDal();
            var task = taskDal.GetTasksForTaskId(taskInfoMod.Id).First();
            var line = mapDal.QueryLine(task.Line.Id);
            var rec = recDal.Query(task.Player.Id);
            var lineMod = new MapLineMod()
            {
                EnName = line.EnName,
                Id = line.Id,
                LineId = line.LineId,
                MapId = line.MapId,
                Name = line.Name
            };
            var recMod = new Preachresource();
            recMod.Id = rec.Id;
            recMod.Name = rec.Name;
            recMod.Desc = rec.Desc;
            recMod.FileSize = rec.FileSize;
            switch (rec.ResourceType)
            {
                case ResourceType.Video:
                    recMod.ResourceType = "视频";
                    break;
                case ResourceType.Voice:
                    recMod.ResourceType = "音频";
                    break;
                case ResourceType.Text:
                    recMod.ResourceType = "文本";
                    break;
                case ResourceType.Picture:
                    recMod.ResourceType = "图片";
                    break;
            }
            recMod.ThumbnailFileInfoPath = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{rec.ThumbnailFileInfoPath}";
            recMod.FileInfoPath = rec.FileInfoPath;
            recMod.Time = rec.Time;
            this.txtLine.Text = $"{lineMod.Name}({lineMod.EnName})";
            this.txtLine.DataContext = lineMod;
            this.txtPlayer.Text = $"{recMod.Name}({ recMod.ResourceType})";
            this.txtPlayer.DataContext = recMod;
            num = 0;
            foreach (var item in tcvm.Cmds)
            {
                if (item.Id.Equals(task.Cmd.Split(' ')[0]))
                {
                    break;
                }
                num++;
            }
            this.cbCmdSelect.SelectedIndex = num;
            this.txtAngle.Text = task.Angle.ToString();
            this.txtCirculateCount.Text = taskInfoMod.CirculateCount.ToString();
        }

        private void PastingEvent(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                e.CancelCommand();
            }
        }

        //选择线
        private void BtnSelectLine_Click(object sender, RoutedEventArgs e)
        {
            var mapid = this.cbMapSelect.SelectedItem as MapResource;
            if (mapid == null)
            {
                MessageBox.Show("请先选择地图");
                return;
            }
            var flyout = new SelectMapLineFlyout(mapid.MapId, Maxvision.Robot.Sdk.Model.LineType.copy);
            flyout.OnSelected += (s) =>
            {
                this.txtLine.Text = $"{s.Name}({s.EnName})";
                this.txtLine.DataContext = s;
            };
            flyout.ShowDialog();
        }
        //选择媒体资源
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
        //确认
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            var name = this.txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请填写名称");
                return;
            }
            var map = this.cbMapSelect.SelectedItem as MapResource;
            if (map == null)
            {
                MessageBox.Show("请先选择地图");
                return;
            }
            var line = this.txtLine.DataContext as MapLineMod;
            if (line == null)
            {
                MessageBox.Show("路线不能为空");
                return;
            }
            var rec = this.txtPlayer.DataContext as Preachresource;
            var cmd = this.cbCmdSelect.SelectedItem as CmdMod;
            if (rec == null && cmd == null || rec == null && cmd != null)
            {
                var c = false;
                if (cmd != null)
                {
                    if (cmd.Id != "无")
                    {
                        c = true;
                    }
                }
                if (!c)
                {
                    MessageBox.Show("资源或命令必须选择其一");
                    return;
                }
            }
            var desc = this.txtDesc.Text;
            var count = int.Parse(string.IsNullOrEmpty(this.txtCirculateCount.Text.Trim()) ? "0" : this.txtCirculateCount.Text.Trim());
            var angle = double.Parse(string.IsNullOrEmpty(this.txtAngle.Text.Trim()) ? "0" : this.txtAngle.Text.Trim());
            var time = 0;
            var task = new RobotTaskManagement();
            var item = new TaskInfoParms()
            {
                Mapid = map.MapId,
                Desc = desc,
                CirculateCount = count == 0 ? int.MaxValue : count,
                Name = name,
                TaskType = TaskType.cruise,
                Tasks = new List<RobotTaskParms>()
            };
            item.Tasks.Add(new RobotTaskParms()
            {
                Angle = (float)angle,
                Cmd = cmd.Id + " " + "无",
                CmdType = CmdType.player,
                Duration = time,
                LineId = line.Id,
                PlayerId = rec.Id,
                PositionId = -1
            });
            var b = false;
            if (_taskInfoMod == null)
                b = task.AddTask(item);
            else
            {
                item.Id = _taskInfoMod.Id;
                b = task.UpdataTask(item);
            }
            if (!b)
            {
                MessageBox.Show("提交失败");
                return;
            }
            this.Close();
            this.Refresh?.Invoke();
        }

        private void TxtCirculateCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]");
            e.Handled = re.IsMatch(e.Text);
        }

        private void TxtTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]");
            e.Handled = re.IsMatch(e.Text);
        }

        private void TxtAngle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9.-]");
            e.Handled = re.IsMatch(e.Text);
        }
    }

}
