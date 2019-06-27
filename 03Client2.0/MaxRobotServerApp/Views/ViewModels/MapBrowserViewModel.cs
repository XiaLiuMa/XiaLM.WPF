using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods;
using Maxvision.Robot.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaxRobotServerApp.Views.ViewModels
{
    public class MapBrowserViewModel
    {
        public MapBrowserViewModel() { }
        private string _mapid = string.Empty;
        public MapBrowserViewModel(string mapid)
        {
            _mapid = mapid;
        }
        public string get_maps()
        {
            MapInfoList mapInfoList = new MapInfoList()
            {
                maps = new List<MapInfo>()
            };
            try
            {
                var dal = new MapResourceDal();
                var list = dal.GetMapInfos(new Maxvision.Robot.Sdk.Model.InPage() { Current = 0, Row = int.MaxValue });
                if (list == null || list.Datas == null || list.Datas.Count <= 0) throw new Exception("没有请求数据");
                var lst2 = list.Datas.Where(r => r.MapId.Equals(_mapid)).ToList();
                if (lst2 != null && lst2.Count > 0)
                {
                    list.Datas.Clear();
                    list.Datas.AddRange(lst2);
                }
                list.Datas.OrderBy(r => r.Id).ToList().ForEach(r =>
            {
                mapInfoList.maps.Add(new MapInfo()
                {
                    az = r.Az,
                    enname = r.EnName,
                    height = r.Height,
                    mapid = r.MapId,
                    maxzoom = r.MaxZoom,
                    minzoom = r.MinZoom,
                    name = r.Name,
                    url = $"http://{RobotClient.Config.Ip}:{RobotClient.Config.WebApiPort}/{r.Url}",
                    width = r.Width,
                    zoom = r.Zoom
                });
            });
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(mapInfoList);
        }
        /// <summary>
        /// 根据地图id得到机器人坐标列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string getrobotpoints(string json)
        {
            var list = new List<RobotPointInfoMpd>();
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var mod = JsonConvert.DeserializeObject<RobotPointInfoMpd>(json);
                return JsonConvert.SerializeObject(RobotPositon.Builder.GetRobotPoints(mod.Mapid));
            }
            catch (Exception)
            {
            }
            return JsonConvert.SerializeObject(list);
        }

        #region 点相关
        public string add_position(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var position = JsonConvert.DeserializeObject<MapPosition>(json);
                var dal = new MapResourceDal();
                var b = dal.AddPosition(new Maxvision.Robot.Sdk.Model.MapPositionInfoParms()
                {
                    Angle = position.angle,
                    Mapid = position.mapid,
                    Name = position.name,
                    PosId = position.posid,
                    PosX = position.posx,
                    PosY = position.posy,
                    Type = (Maxvision.Robot.Sdk.Model.PositionType)position.type,
                    Zoom = position.zoom
                });
                if (b)
                {
                    return JsonConvert.SerializeObject(new { resultdata = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { resultdata = 0 });
        }
        public string update_position(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var position = JsonConvert.DeserializeObject<MapPosition>(json);
                var dal = new MapResourceDal();
                var b = dal.UpdataPosition(new Maxvision.Robot.Sdk.Model.MapPositionInfoParms()
                {
                    PosId = position.posid,
                    Name = position.name,
                    Type = (Maxvision.Robot.Sdk.Model.PositionType)position.type,
                    Zoom = position.zoom,
                    PosX = position.posx,
                    PosY = position.posy,
                    Angle = position.angle,
                    Mapid = position.mapid
                });
                if (b)
                {
                    return JsonConvert.SerializeObject(new { resultdata = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { resultdata = 0 });
        }
        public string delete_position(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var position = JsonConvert.DeserializeObject<MapPosition>(json);
                var dal = new MapResourceDal();
                var pos = dal.QueryPositionForPosid(position.posid);
                if (pos != null)
                {
                    var taskdal = new TaskMangerDal();
                    var c = taskdal.IsPositionUsed(pos.Id);
                    if (c)
                    {
                        MessageBox.Show($"点({pos.Name})已在任务中配置，如需删除请先解除关联！");
                        return JsonConvert.SerializeObject(new { resultdata = 0 });
                    }
                }
                var b = dal.DeletePosition(position.posid);
                if (b)
                {
                    return JsonConvert.SerializeObject(new { resultdata = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { resultdata = 0 });
        }
        public string get_positions(string json)
        {
            var mapPositionList = new MapPositionList()
            {
                positions = new List<MapPosition>()
            };

            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var position = JsonConvert.DeserializeObject<MapPosition>(json);
                var dal = new MapResourceDal();
                var list = dal.GetMapPositionInfoList(new Maxvision.Robot.Sdk.Model.InPage() { Current = 0, Row = int.MaxValue }, position.mapid);
                if (list == null || list.Datas == null || list.Datas.Count <= 0) throw new Exception("没有请求数据");
                list.Datas.ForEach(r =>
                {
                    mapPositionList.positions.Add(new MapPosition()
                    {
                        angle = r.Angle,
                        mapid = r.Mapid,
                        name = r.Name,
                        posid = r.PosId,
                        posx = r.PosX,
                        posy = r.PosY,
                        type = (int)r.Type,
                        zoom = r.Zoom
                    });
                });
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(mapPositionList);
        }

        #endregion

        #region 线相关
        public string add_path(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var line = JsonConvert.DeserializeObject<MapLine>(json);
                var dal = new MapResourceDal();
                var b = dal.AddMapLineInfo(new Maxvision.Robot.Sdk.Model.MapLineInfoParms()
                {
                    EnName = Guid.NewGuid().ToString().Replace("-", ""),
                    LineData = line.linedata,
                    LineId = line.lineid,
                    MapId = line.mapid,
                    Name = line.name,
                    Points = line.points,
                    Task = line.task,
                    TaskData = line.taskdata,
                    Type = line.type
                });
                if (b)
                {
                    return JsonConvert.SerializeObject(new { resultdata = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { resultdata = 0 });
        }
        public string update_path(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var line = JsonConvert.DeserializeObject<MapLine>(json);
                var dal = new MapResourceDal();
                var b = dal.UpdataMapLineInfo(new Maxvision.Robot.Sdk.Model.MapLineInfoParms()
                {
                    LineData = line.linedata,
                    LineId = line.lineid,
                    MapId = line.mapid,
                    Name = line.name,
                    Points = line.points,
                    Task = line.task,
                    TaskData = line.taskdata,
                    Type = line.type
                });
                if (b)
                {
                    return JsonConvert.SerializeObject(new { resultdata = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { resultdata = 0 });
        }
        public string delete_path(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var line = JsonConvert.DeserializeObject<MapLine>(json);
                var dal = new MapResourceDal();
                var line2 = dal.QueryLineForLineid(line.lineid);
                if (line2 != null)
                {
                    var taskdal = new TaskMangerDal();
                    var c = taskdal.IsMapLineUsed(line2.Id);
                    if (c)
                    {
                        MessageBox.Show($"线({line2.Name})已在任务中配置，如需删除请先解除关联！");
                        return JsonConvert.SerializeObject(new { resultdata = 0 });
                    }
                }
                var b = dal.DeleteMapLineInfo(line.lineid);
                if (b)
                {
                    return JsonConvert.SerializeObject(new { resultdata = 1 });
                }
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(new { resultdata = 0 });
        }
        public string get_paths(string json)
        {
            var mapLineList = new MapLineList()
            {
                paths = new List<MapLine>()
            };

            try
            {
                if (string.IsNullOrEmpty(json)) throw new Exception("参数为空");
                var line = JsonConvert.DeserializeObject<MapLine>(json);
                var dal = new MapResourceDal();
                var list = dal.GetMapLineInfoList(new Maxvision.Robot.Sdk.Model.InPage() { Current = 0, Row = int.MaxValue }, Maxvision.Robot.Sdk.Model.LineType.major, line.mapid);
                if (list == null || list.Datas == null || list.Datas.Count <= 0) throw new Exception("没有请求数据");
                list.Datas.ForEach(r =>
                {
                    mapLineList.paths.Add(new MapLine()
                    {
                        linedata = r.LineData,
                        lineid = r.LineId,
                        mapid = r.MapId,
                        name = r.Name,
                        points = r.Points,
                        task = r.Task,
                        taskdata = r.TaskData,
                        type = r.Type
                    });
                });
            }
            catch (Exception ex)
            {
            }
            return JsonConvert.SerializeObject(mapLineList);
        }
        #endregion
    }
    public class RobotPositon
    {
        private static RobotPositon realize;
        private static readonly object objlock = new object();
        public static RobotPositon Builder => new Func<RobotPositon>(() =>
        {
            if (realize == null)
            {
                lock (objlock)
                {
                    if (realize == null)
                    {
                        realize = new RobotPositon();
                    }
                }
            }
            return realize;
        })();
        private ObjectCache cache;
        private CacheItemPolicy policy;
        public RobotPositon()
        {

        }
        public void Init()
        {
            cache = new MemoryCache(Guid.NewGuid().ToString());
            policy = new CacheItemPolicy();
            policy.RemovedCallback += s =>
            {

            };
            var robot = new RobotManagement();
            robot.Online += s =>
            {
                if (s.Position == null) return;
                var obj = cache.Get(s.Position.Mapid);
                if (obj == null)
                {
                    policy.SlidingExpiration = new TimeSpan(0, 0, 0, 10);
                    var list = new List<RobotPointInfoMpd>();
                    list.Add(new RobotPointInfoMpd() { Mapid = s.Position.Mapid, Name = robot.GetRobotInfo(s.Tag).Name, Tag = s.Tag, X = s.Position.X, Y = s.Position.Y, Angle = s.Position.Angle });
                    cache.Add(s.Position.Mapid, list, policy);
                }
                else
                {
                    var pos = (obj as List<RobotPointInfoMpd>);
                    if (pos != null)
                    {
                        var r1 = pos.Where(r => r.Tag.Equals(s.Tag)).FirstOrDefault();
                        if (r1 == null)
                        {
                            pos.Add(new RobotPointInfoMpd() { Mapid = s.Position.Mapid, Name = robot.GetRobotInfo(s.Tag).Name, Tag = s.Tag, X = s.Position.X, Y = s.Position.Y });
                        }
                        else
                        {
                            r1.Mapid = s.Position.Mapid;
                            r1.Name = robot.GetRobotInfo(s.Tag).Name;
                            r1.Tag = s.Tag;
                            r1.X = s.Position.X;
                            r1.Y = s.Position.Y;
                            r1.Angle = s.Position.Angle;
                        }
                    }
                }
            };
        }
        public List<RobotPointInfoMpd> GetRobotPoints(string mapid)
        {
            var list = new List<RobotPointInfoMpd>();
            var item = cache.Get(mapid);
            if (item == null) return list;
            var obj = item as List<RobotPointInfoMpd>;
            if (obj == null) return list;
            return obj;
        }
    }
}
