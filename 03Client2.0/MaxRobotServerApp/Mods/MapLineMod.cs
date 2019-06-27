using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    /// <summary>
    /// 地图线
    /// </summary>
    public class MapLineMod
    {
        public int Id { get; set; }
        /// <summary>
        /// 唯一的路线编号，用于区分路线信息
        /// </summary>
        public string LineId { get; set; }
        /// <summary>
        /// 路线名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnName { get; set; }
        /// <summary>
        /// 地图编号
        /// </summary>
        public string MapId { get; set; }
    }
}
