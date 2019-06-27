using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    public class SubTaskMod
    {
        public int Id { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        public float Angle { get; set; }


        public int PositionId { get; set; }

        public int LineId { get; set; }
        /// <summary>
        /// 点或线的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string RecName { get; set; }
        /// <summary>
        /// 停留时长
        /// </summary>
        public int Duration { get; set; }
        public string CmdTypeStr { get; set; }
        public string CmdStr { get; set; }
        /// <summary>
        /// 指令类型，1:动作（包含动作命令，功能命令，参考commandwords.json文件），2:音视频播放，此时下载媒体资源
        /// </summary>
        public CmdType CmdType { get; set; }
        public string Cmd { get; set; }
        public int PlayerId { get; set; }
    }
}
