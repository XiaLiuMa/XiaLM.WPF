using System.Collections.Generic;
using XiaLM.Owin.FileSever;

namespace XiaLM.Owin
{
    /// <summary>
    /// Owin启动类
    /// </summary>
    public class OwinStart
    {
        /// <summary>
        /// 是否开启Signalr
        /// </summary>
        public bool IsOpenSignalR { get; set; }
        /// <summary>
        /// 是否开启webapi
        /// </summary>
        public bool IsOpenWebApi { get; set; }
        /// <summary>
        /// 文件服务器基本配置
        /// </summary>
        public FileServerOptions FileServerOptions { get; set; }
        /// <summary>
        /// 是否跨域
        /// </summary>
        public bool IsCorss { get; set; }
        /// <summary>
        /// 路由列表
        /// </summary>
        public IEnumerable<HttpRoute> Routes { get; set; }
        /// <summary>
        /// 结果是否为Json
        /// </summary>
        public bool ResultToJson { get; set; }
    }
}
