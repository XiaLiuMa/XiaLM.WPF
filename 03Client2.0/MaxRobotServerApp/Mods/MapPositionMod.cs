using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    public class MapPositionMod
    {
        public int Id { get; set; }
        /// <summary>
        /// 导航点编号guid
        /// </summary>
        public string PosId { get; set; }
        /// <summary>
        /// 名称描述
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnName { get; set; }
        /// <summary>
        /// 地图编号
        /// </summary>
        public string Mapid { get; set; }

    }
}
