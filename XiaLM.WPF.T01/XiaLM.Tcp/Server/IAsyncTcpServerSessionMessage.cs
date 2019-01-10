using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Server
{
    public interface IAsyncTcpServerSessionMessage
    {
        /// <summary>
        /// 客户端已连接
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task OnSessionStarted(AsyncTcpServerSession session);
        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="session"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task OnSessionDataReceived(AsyncTcpServerSession session, byte[] data, int offset, int count);
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task OnSessionClosed(AsyncTcpServerSession session);
        /// <summary>
        /// 错误消息输出
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        Task OnSessionError(Exception ex);
    }

    public delegate Task SessionStarted(AsyncTcpServerSession session);
    public delegate Task SessionDataReceived(AsyncTcpServerSession session, byte[] data, int offset, int count);
    public delegate Task SessionClosed(AsyncTcpServerSession session);
    public delegate Task SessionError(Exception ex);
}
