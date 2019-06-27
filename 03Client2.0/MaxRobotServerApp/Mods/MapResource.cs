using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    public class MapResource
    {
        public int Id { get; set; }
        /// <summary>
        /// 地图唯一编号，地图
        /// </summary>
        public string MapId { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地图英文名称
        /// </summary>
        public string EnName { get; set; }
        /// <summary>
        /// 地图图片Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 最小缩放等级
        /// </summary>
        public int MinZoom { get; set; }
        /// <summary>
        /// 最大缩放等级
        /// </summary>
        public int MaxZoom { get; set; }
        /// <summary>
        /// 当前等级
        /// </summary>
        public int Zoom { get; set; }
        /// <summary>
        /// 地图角度
        /// </summary>
        public float Az { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
