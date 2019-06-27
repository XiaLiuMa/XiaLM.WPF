using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    public class CmdMod
    {
        public int Num { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 是否属于功能
        /// </summary>
        public bool IsFunctions { get; set; }
    }
}
