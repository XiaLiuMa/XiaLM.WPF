using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class MapResourceDal
    {
        private MapManagement mapManagement;
        public MapResourceDal()
        {
            mapManagement = new MapManagement();
        }
        #region 地图相关
        /// <summary>
        /// 添加地图信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool AddMapInfo(MapInfoParms info)
        {
            return mapManagement.AddMapInfo(info);
        }
        /// <summary>
        /// 修改地图信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdataMapInfo(MapInfoParms info)
        {
            return mapManagement.UpdataMapInfo(info);
        }
        /// <summary>
        /// 删除地图信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteMapInfo(int[] ids)
        {
            return mapManagement.DeleteMapInfo(ids);
        }
        /// <summary>
        /// 获取地图信息列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<MapInfo> GetMapInfos(InPage inPage)
        {
            return mapManagement.GetMapInfos(inPage);
        }
        /// <summary>
        /// 判断英文名称是否存在
        /// </summary>
        /// <param name="enName"></param>
        /// <returns></returns>
        public bool ExitsEnName(string enName)
        {
            return mapManagement.ExitsEnName(enName);
        }
        /// <summary>
        /// 判断mapid是否存在
        /// </summary>
        /// <param name="mapid"></param>
        /// <returns></returns>
        public bool ExitsMapId(string mapid)
        {
            return mapManagement.ExitsMapId(mapid);
        }
        #endregion

        #region 坐标相关
        public MapPositionInfo QueryPositionForPosid(string posid)
        {
            return mapManagement.QueryPositionForPosid(posid);
        }
        public MapPositionInfo QueryPosition(int id)
        {
            return mapManagement.QueryPosition(id);
        }
        /// <summary>
        /// 得到点集合,根据地图编号得到点集合
        /// </summary>
        /// <param name="inPage"></param>
        /// <param name="mapid"></param>
        /// <returns></returns>
        public OutPage<MapPositionInfo> GetMapPositionInfoList(InPage inPage, string mapid)
        {
            return mapManagement.GetMapPositionInfoList(inPage, mapid);
        }
        /// <summary>
        /// 添加坐标点信息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool AddPosition(MapPositionInfoParms parms)
        {
            return mapManagement.AddPosition(parms);
        }
        /// <summary>
        /// 更新坐标点信息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool UpdataPosition(MapPositionInfoParms parms)
        {
            return mapManagement.UpdataPosition(parms);
        }
        /// <summary>
        /// 删除坐标点信息
        /// </summary>
        /// <param name="posid">坐标id</param>
        /// <returns></returns>
        public bool DeletePosition(string posid)
        {
            return mapManagement.DeletePosition(posid);
        }
        #endregion

        #region 导航线相关
        public MapLineInfo QueryLineForLineid(string lineid)
        {
            return mapManagement.QueryLineForLineid(lineid);
        }
        public MapLineInfo QueryLine(int id)
        {

            return mapManagement.QueryLine(id);
        }
        /// <summary>
        /// 得到线的集合
        /// </summary>
        /// <param name="inPage"></param>
        /// <param name="mapid"></param>
        /// <returns></returns>
        public OutPage<MapLineInfo> GetMapLineInfoList(InPage inPage, LineType lineType, string mapid)
        {
            return mapManagement.GetMapLineInfoList(inPage, lineType, mapid);
        }
        /// <summary>
        /// 添加导航线
        /// </summary>
        /// <returns></returns>
        public bool AddMapLineInfo(MapLineInfoParms parms)
        {
            return mapManagement.AddMapLineInfo(parms);
        }
        /// <summary>
        /// 更新导航线信息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool UpdataMapLineInfo(MapLineInfoParms parms)
        {
            return mapManagement.UpdataMapLineInfo(parms);
        }
        /// <summary>
        /// 删除导航线
        /// </summary>
        /// <param name="lineid"></param>
        /// <returns></returns>
        public bool DeleteMapLineInfo(string lineid)
        {
            return mapManagement.DeleteMapLineInfo(lineid);
        }
        #endregion
    }
}
