using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaxRobotServerApp.Mods
{
    public class Robot
    {
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; } = false;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 网络摄像头ip
        /// </summary>
        public string IPCameraIp { get; set; }
        /// <summary>
        /// 网络摄像头端口
        /// </summary>
        public int IPCameraPort { get; set; }
        /// <summary>
        /// 红外摄像头ip
        /// </summary>
        public string InfraredCameraIP { get; set; }
        /// <summary>
        /// 红外摄像头端口
        /// </summary>
        public int InfraredCameraPort { get; set; }
    }
}
