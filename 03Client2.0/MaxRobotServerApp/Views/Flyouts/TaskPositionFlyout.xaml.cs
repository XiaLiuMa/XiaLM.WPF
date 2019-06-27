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
using System.Windows.Controls;
using System.Windows.Input;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// TaskPositionFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class TaskPositionFlyout : CBaseFlyout
    {
        public TaskPositionFlyout()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new TaskConfigurationViewModel();
        }
        private Action Refresh;
        private TaskInfoMod _taskInfoMod;
        public TaskPositionFlyout(Action refresh) : this()
        {
            Refresh = refresh;
        }
        public TaskPositionFlyout(Action refresh, TaskInfoMod taskInfoMod) : this()
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
            this.txtCirculateCount.Text = taskInfoMod.CirculateCount.ToString();
            var mapDal = new MapResourceDal();
            var taskDal = new TaskMangerDal();
            var recDal = new PreachresourceDal();
            var tasks = taskDal.GetTasksForTaskId(taskInfoMod.Id).OrderByDescending(r => r.Id);
            int i = 1;
            foreach (var task in tasks)
            {
                var position = mapDal.QueryPosition(task.Position.Id);
                ResourceInfo rec = null;
                if(task.Player!=null)
                 rec = recDal.Query(task.Player.Id);
                var positionMod = new MapPositionMod()
                {
                    Name = position.Name,
                    EnName = position.EnName,
                    Id = position.Id,
                    Mapid = position.Mapid,
                    PosId = position.PosId
                };
                if (rec != null)
                {
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
                }

                tcvm.SubTaskMods.Add(new SubTaskMod()
                {
                    Id = task.Id,
                    Angle = task.Angle,
                    Cmd = task.Cmd,
                    CmdStr = tcvm.Cmds.Where(r => r.Id == task.Cmd.Split(' ')[0]).First().Name + " " + tcvm.Functions.Where(r => r.Id == task.Cmd.Split(' ')[1]).First().Name,
                    CmdType = task.CmdType,
                    CmdTypeStr = task.CmdType == CmdType.function ? "执行功能" : "播放媒体",
                    Duration = task.Duration,
                    LineId = -1,
                    Name = positionMod.Name + $"({positionMod.EnName})",
                    Number = i,
                    PlayerId = rec == null ? -1 : rec.Id,
                    PositionId = task.Position.Id,
                    RecName = rec == null ? "" : rec.Name
                });
                i++;
            }
        }
        //确定
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
            var desc = this.txtDesc.Text;
            var count = int.Parse(string.IsNullOrEmpty(this.txtCirculateCount.Text.Trim()) ? "0" : this.txtCirculateCount.Text.Trim());
            var tasks = (DataContext as TaskConfigurationViewModel).SubTaskMods;
            if (tasks == null || tasks.Count <= 0)
            {
                MessageBox.Show("未添加任何子任务，请添加子任务");
                return;
            }
            var task = new RobotTaskManagement();
            var item = new TaskInfoParms()
            {
                Mapid = map.MapId,
                Desc = desc,
                CirculateCount = count == 0 ? int.MaxValue : count,
                Name = name,
                TaskType = TaskType.point,
                Tasks = new List<RobotTaskParms>()
            };
            foreach (var t in tasks)
            {
                item.Tasks.Add(new RobotTaskParms()
                {
                    Angle = t.Angle,
                    Cmd = t.Cmd,
                    CmdType = t.CmdType,
                    Duration = t.Duration,
                    LineId = t.LineId,
                    PlayerId = t.PlayerId,
                    PositionId = t.PositionId
                });
            }
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
        //删除子任务
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var sub = (((Button)sender).DataContext as SubTaskMod);
            var tasks = (DataContext as TaskConfigurationViewModel).SubTaskMods;
            var list = new List<SubTaskMod>();
            foreach (var task in tasks)
            {
                list.Add(task);
            }
            tasks.Clear();
            list.Remove(sub);
            var num = 1;
            foreach (var task in list)
            {
                task.Number = num;
                tasks.Add(task);
                num++;
            }
        }
        //编辑子任务
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var sub = (((Button)sender).DataContext as SubTaskMod);
            var map = this.cbMapSelect.SelectedItem as MapResource;
            if (map == null)
            {
                MessageBox.Show("请先选择地图");
                return;
            }
            var flyout = new AddMapPositionSubtaskFlyout(map, true, sub);

            flyout.OnAdded += s =>
            {

                var item = sub;
                var tasks = (DataContext as TaskConfigurationViewModel).SubTaskMods;
                var list = new List<SubTaskMod>();
                foreach (var task in tasks)
                {
                    list.Add(task);
                }
                tasks.Clear();
                item.Angle = s.Angle;
                item.Cmd = s.Cmd;
                item.CmdStr = s.CmdStr;
                item.CmdType = s.CmdType;
                item.CmdTypeStr = s.CmdTypeStr;
                item.Duration = s.Duration;
                item.LineId = s.LineId;
                item.Name = s.Name;
                item.Number = s.Number;
                item.PlayerId = s.PlayerId;
                item.PositionId = s.PositionId;
                item.RecName = s.RecName;
                var num = 1;
                foreach (var task in list)
                {
                    task.Number = num;
                    tasks.Add(task);
                    num++;
                }
                list.Clear();
            };
            flyout.OnError += () => {
                MessageBox.Show("数据加载失败");
                flyout.Close();
            };
            flyout.ShowDialog();
            (DataContext as TaskConfigurationViewModel).SubTaskMods.OrderBy(r => r.Number);
        }
        //添加子任务
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            var map = this.cbMapSelect.SelectedItem as MapResource;
            var flyout = new AddMapPositionSubtaskFlyout(map);
            flyout.OnAdded += s =>
            {
                var tasks = (DataContext as TaskConfigurationViewModel).SubTaskMods;
                var list = new List<SubTaskMod>();
                foreach (var task in tasks)
                {
                    list.Add(task);
                }
                tasks.Clear();
                var num = 1;
                foreach (var item in list)
                {
                    item.Number = num;
                    tasks.Add(item);
                    num++;
                }
                s.Number = num;
                tasks.Add(s);
                list.Clear();
            };
            flyout.ShowDialog();
        }

        private void TxtCirculateCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var re = new Regex("[^0-9]");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
