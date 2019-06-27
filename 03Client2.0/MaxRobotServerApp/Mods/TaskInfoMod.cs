using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
   public class TaskInfoMod
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// 任务类型描述
        /// </summary>
        public string TaskTypeStr { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskType TaskType { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 地图名称
        /// </summary>
        public string Mapid { get; set; }
        /// <summary>
        /// 循环次数
        /// </summary>
        public int CirculateCount { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Time { get; set; }
    }
  
}
