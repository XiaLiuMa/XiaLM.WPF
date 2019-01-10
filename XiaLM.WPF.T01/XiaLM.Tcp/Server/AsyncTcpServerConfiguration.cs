using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Server
{
    public class AsyncTcpServerConfiguration
    {
        public AsyncTcpServerConfiguration()
        {
            ReceiveBufferSize = 8192;                   // Specifies the total per-socket buffer space reserved for receives. This is unrelated to the maximum message size or the size of a TCP window.
            SendBufferSize = 8192;                      // Specifies the total per-socket buffer space reserved for sends. This is unrelated to the maximum message size or the size of a TCP window.
            ReceiveTimeout = TimeSpan.Zero;             // Receive a time-out. This option applies only to synchronous methods; it has no effect on asynchronous methods such as the BeginSend method.
            SendTimeout = TimeSpan.Zero;                // Send a time-out. This option applies only to synchronous methods; it has no effect on asynchronous methods such as the BeginSend method.
            NoDelay = true;                             // Disables the Nagle algorithm for send coalescing.
            LingerState = new LingerOption(false, 0);   // The socket will linger for x seconds after Socket.Close is called.
            KeepAlive = false;                          // Use keep-alives.
            KeepAliveInterval = TimeSpan.FromSeconds(5);// https://msdn.microsoft.com/en-us/library/system.net.sockets.socketoptionname(v=vs.110).aspx
            ReuseAddress = false;                       // Allows the socket to be bound to an address that is already in use.
            ConnectTimeout = TimeSpan.FromSeconds(15);
        }
        public TimeSpan ConnectTimeout { get; set; }
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int BacklogConnections { get; set; }
        /// <summary>
        /// 是否启动网络地址转换
        /// </summary>
        public bool AllowNatTraversal { get; set; }
        /// <summary>
        /// 是否允许将套接字绑定到已在使用中的地址
        /// </summary>
        public bool ReuseAddress { get; set; }
        /// <summary>
        /// 接收缓冲区的大小（以字节为单位）。 默认值为 8192 个字节。
        /// </summary>
        public int ReceiveBufferSize { get; set; }
        /// <summary>
        /// 发送缓冲区的大小（以字节为单位）。 默认值为 8192 个字节。
        /// </summary>
        public int SendBufferSize { get; set; }
        /// <summary>
        /// 连接的超时值（以毫秒为单位）。 默认值为 0。
        /// </summary>
        public TimeSpan ReceiveTimeout { get; set; }
        /// <summary>
        /// 发送超时值（以毫秒为单位）。 默认值为 0。
        /// </summary>
        public TimeSpan SendTimeout { get; set; }
        /// <summary>
        /// 如果禁用延迟，则为 true；否则为 false。 默认值为 false。
        /// </summary>
        public bool NoDelay { get; set; }
        /// <summary>
        /// 获取或设置有关关联的套接字的延迟状态的信息。
        /// </summary>
        public LingerOption LingerState { get; set; }
        /// <summary>
        /// 是否保持连接
        /// </summary>
        public bool KeepAlive { get; set; }
        /// <summary>
        /// 保持连接间隔
        /// </summary>
        public TimeSpan KeepAliveInterval { get; set; }
    }
}
