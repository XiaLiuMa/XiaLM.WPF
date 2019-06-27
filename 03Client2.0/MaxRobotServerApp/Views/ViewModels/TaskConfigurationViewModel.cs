using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using Maxvision.Robot.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class TaskConfigurationViewModel : Ext.ViewModel
    {
        public List<MapResource> MapInfos { get; }
        public List<CmdMod> Cmds { get; }
        public List<CmdMod> Functions { get; }
        public ObservableCollection<SubTaskMod> SubTaskMods { get; }

        public TaskConfigurationViewModel()
        {
            MapInfos = new List<MapResource>();
            Cmds = new List<CmdMod>();
            Functions = new List<CmdMod>();
            SubTaskMods = new ObservableCollection<SubTaskMod>();
            Init();
        }

        private void Init()
        {
            var map = new MapResourceDal();
            var datas = map.GetMapInfos(new Maxvision.Robot.Sdk.Model.InPage() { Current = 1, Row = int.MaxValue }).Datas;
            foreach (var item in datas)
            {
                MapInfos.Add(new MapResource()
                {
                    Az = item.Az,
                    EnName = item.EnName,
                    Id = item.Id,
                    MapId = item.MapId,
                    MaxZoom = item.MaxZoom,
                    MinZoom = item.MinZoom,
                    Name = item.Name,
                    Url = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{item.Url}",
                    Zoom = item.Zoom
                });
            }
            Cmds.AddRange(new CmdMod[] {
                   //new CmdMod{Num=0,Id="无",Name="无" },
                   //new CmdMod{Num=1,Id="C101",Name="摇头" },
                   //new CmdMod{Num=2,Id="C102",Name="点头" },
                   //new CmdMod{Num=3, Id="C103",Name="抬头"},
                   //new CmdMod{Num=4,Id="C104",Name="低头" },
                   //new CmdMod{Num=5,Id="C105",Name="向左看" },
                   //new CmdMod{Num=6, Id="C106",Name="向右看"},
                   //new CmdMod{Num=7,Id="C201",Name="抬手" },
                   //new CmdMod{Num=8, Id="C202",Name="抬左手"},
                   //new CmdMod{Num=9,Id="C203",Name="抬右手" },
                   //new CmdMod{Num=10, Id="C204",Name="引路"},
                   //new CmdMod{ Num=11, Id="C205",Name="握手"},
                   //new CmdMod{Num=12,Id="C206",Name="敬礼" },


                    new CmdMod{Num=0,Id="无",Name="无" },
                   new CmdMod{Num=1,Id="C101",Name="摇头" },
                   new CmdMod{Num=2,Id="C102",Name="点头" },
                   new CmdMod{Num=3, Id="C103",Name="抬头"},
                   new CmdMod{Num=4,Id="C104",Name="低头" },
                   new CmdMod{Num=5,Id="C105",Name="向左看" },
                   new CmdMod{Num=6, Id="C106",Name="向右看"},
                   new CmdMod{Num=7,Id="C201",Name="抬手" },
                   new CmdMod{Num=8, Id="C202",Name="抬左手"},
                   new CmdMod{Num=9,Id="C203",Name="抬右手" },
                   new CmdMod{Num=10, Id="C204",Name="引路"},
                   new CmdMod{ Num=11, Id="C205",Name="握手"},
                   new CmdMod{Num=12,Id="C206",Name="敬礼" },


                   new CmdMod{Num=14-1,Id="C401",Name="宣传互动" },
                   new CmdMod{ Num=15-1,Id="C403",Name="咨询服务"},
                   new CmdMod{ Num=16-1,Id="C405",Name="地图导航"},
                   new CmdMod{ Num=17-1,Id="C407",Name="跟随"},
                   new CmdMod{Num=18-1, Id="C409",Name="区域布控"},
                   new CmdMod{Num=19-1, Id="C411",Name="低温探测"},
                   new CmdMod{Num=20-1,Id="C413",Name="智能判图" },
                   new CmdMod{Num=21-1,Id="C417",Name="翻译" },
                   new CmdMod{Num=22-1,Id="C504",Name="充电" }

            });
            Functions.AddRange(new CmdMod[] {
                   //new CmdMod{Num=0,Id="无",Name="无" },
                   //new CmdMod{Num=1,Id="C401",Name="宣传互动" },
                   //new CmdMod{ Num=2,Id="C403",Name="咨询服务"},
                   //new CmdMod{ Num=3,Id="C405",Name="地图导航"},
                   //new CmdMod{ Num=4,Id="C407",Name="跟随"},
                   //new CmdMod{Num=5, Id="C409",Name="区域布控"},
                   //new CmdMod{Num=6, Id="C411",Name="低温探测"},
                   //new CmdMod{Num=7,Id="C413",Name="智能判图" },
                   //new CmdMod{Num=8,Id="C417",Name="翻译" },
                   //new CmdMod{Num=9,Id="C504",Name="充电" }


                  new CmdMod{Num=0,Id="无",Name="无" },
                   new CmdMod{Num=1,Id="C101",Name="摇头" },
                   new CmdMod{Num=2,Id="C102",Name="点头" },
                   new CmdMod{Num=3, Id="C103",Name="抬头"},
                   new CmdMod{Num=4,Id="C104",Name="低头" },
                   new CmdMod{Num=5,Id="C105",Name="向左看" },
                   new CmdMod{Num=6, Id="C106",Name="向右看"},
                   new CmdMod{Num=7,Id="C201",Name="抬手" },
                   new CmdMod{Num=8, Id="C202",Name="抬左手"},
                   new CmdMod{Num=9,Id="C203",Name="抬右手" },
                   new CmdMod{Num=10, Id="C204",Name="引路"},
                   new CmdMod{ Num=11, Id="C205",Name="握手"},
                   new CmdMod{Num=12,Id="C206",Name="敬礼" },


                   new CmdMod{Num=14-1,Id="C401",Name="宣传互动" },
                   new CmdMod{ Num=15-1,Id="C403",Name="咨询服务"},
                   new CmdMod{ Num=16-1,Id="C405",Name="地图导航"},
                   new CmdMod{ Num=17-1,Id="C407",Name="跟随"},
                   new CmdMod{Num=18-1, Id="C409",Name="区域布控"},
                   new CmdMod{Num=19-1, Id="C411",Name="低温探测"},
                   new CmdMod{Num=20-1,Id="C413",Name="智能判图" },
                   new CmdMod{Num=21-1,Id="C417",Name="翻译" },
                   new CmdMod{Num=22-1,Id="C504",Name="充电" }
            });

        }
    }
}
