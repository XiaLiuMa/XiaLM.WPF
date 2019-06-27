using Maxvision.Robot.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods
{
    public class DownloadInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 进度值
        /// </summary>
        public double ProgressSize { get; set; } = 0;
        /// <summary>
        /// 百分比
        /// </summary>
        public string Percentage { get; set; } = (0 / 100.0).ToString("P2");
        public string State { get; set; }

        public CancellationTokenSource Cts { get; set; }

        public FileDownload Download { get; set; }
    }
}
