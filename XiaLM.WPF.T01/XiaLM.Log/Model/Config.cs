using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Log.Model
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class Config
    {
        public bool CanWriteLog { get; set; }
        public bool CanSendLog { get; set; }
        public UdpServer UdpServer { get; set; }
    }
    /// <summary>
    /// UDP服务端
    /// </summary>
    public class UdpServer
    {
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
    }
}
