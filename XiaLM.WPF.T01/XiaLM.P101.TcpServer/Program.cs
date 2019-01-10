using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Tcp.Codecs;
using XiaLM.Tcp.Server;

namespace XiaLM.P101.TcpServer
{
    class Program
    {
        private static AsyncTcpServer server;
        static void Main(string[] args)
        {
            server = new AsyncTcpServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234), null, new FixedPackageHeaderCodecBuilder(true));
            server.OnSessionError += Server_OnSessionError;
            server.OnSessionClosed += Server_OnSessionClosed;
            server.OnSessionDataReceived += Server_OnSessionDataReceived;
            server.OnSessionStarted += Server_OnSessionStarted;
            server.Listener();
            Console.ReadKey();
        }

        private static async Task Server_OnSessionStarted(AsyncTcpServerSession session)
        {
            Console.WriteLine(session.RemoteEndPoint + "已连接上");
            Console.WriteLine("发送数据到" + session.RemoteEndPoint);
            //await server.SendAsync(session.SessionKey, File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "PersonalCode.zip"));
            for (int p = 0; p < 3; p++)
                for (int i = 0; i < 100; i++)
                {
                    await server.SendAsync(session.SessionKey, Encoding.UTF8.GetBytes(i.ToString()));
                }
        }

        private static async Task Server_OnSessionDataReceived(AsyncTcpServerSession session, byte[] data, int offset, int count)
        {

        }

        private static async Task Server_OnSessionClosed(AsyncTcpServerSession session)
        {
            Console.WriteLine(session.RemoteEndPoint + "已断开上");
        }

        private static async Task Server_OnSessionError(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
