using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Tcp.Client
{
    public interface IAsyncTcpClientMessage
    {
        Task OnServerConnected(AsyncTcpClient client);
        Task OnServerDataReceived(AsyncTcpClient client, byte[] data, int offset, int count);
        Task OnServerDisconnected(AsyncTcpClient client);
        Task OnServerError(Exception ex);
    }
    public delegate Task ServerConnected(AsyncTcpClient client);
    public delegate Task ServerDataReceived(AsyncTcpClient client, byte[] data, int offset, int count);
    public delegate Task ServerDisconnected(AsyncTcpClient client);
    public delegate Task ServerError(Exception ex);
}
