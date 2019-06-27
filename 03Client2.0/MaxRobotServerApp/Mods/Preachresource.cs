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
    /// 宣讲资源
    /// </summary>
    public class Preachresource
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public ulong FileSize { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string ResourceType { get; set; }
        /// <summary>
        /// 缩略图文件地址
        /// </summary>
        public string ThumbnailFileInfoPath { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileInfoPath { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
