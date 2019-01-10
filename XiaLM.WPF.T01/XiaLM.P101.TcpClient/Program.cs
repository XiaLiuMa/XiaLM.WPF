using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Tcp.Client;
using XiaLM.Tcp.Codecs;

namespace XiaLM.P101.TcpClient
{
    class Program
    {
        private static AsyncTcpClient tcpClient;
        static void Main(string[] args)
        {
            tcpClient = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234), null, new FixedPackageHeaderCodecBuilder(true));
            tcpClient.OnServerConnected += TcpClient_OnServerConnected;
            tcpClient.OnServerDataReceived += TcpClient_OnServerDataReceived;
            tcpClient.OnServerDisconnected += TcpClient_OnServerDisconnected;
            tcpClient.OnServerError += TcpClient_OnServerError;
            tcpClient.Connect().Wait();
            Console.ReadKey();
        }

        private static async Task TcpClient_OnServerError(Exception ex)
        {
            Console.WriteLine(ex);
        }

        private static async Task TcpClient_OnServerDisconnected(AsyncTcpClient client)
        {
            Console.WriteLine("服务端连接断开");
        }

        private static async Task TcpClient_OnServerDataReceived(AsyncTcpClient client, byte[] data, int offset, int count)
        {
            //File.WriteAllBytes(@"E:\Temp\1.zip", data);
            Console.Write(Encoding.UTF8.GetString(data) + " ");
        }

        private static async Task TcpClient_OnServerConnected(AsyncTcpClient client)
        {
            Console.WriteLine("连接上服务端");
        }
    }
}
