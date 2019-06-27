using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MaxRobotServerApp.Mods
{
    /// <summary>
    /// App资源
    /// </summary>
    public class Appresource
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 相对路径
        /// </summary>
        public string AppFileUrl { get; set; }
        /// <summary>
        /// 更新方式
        /// </summary>
        public string UpdateMode { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
