using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.WPF.T01.Net.Models
{
    public class AlarmInfo
    {
        /// <summary>
        /// 报警编号
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 机器人
        /// </summary>
        public int RobotId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}
