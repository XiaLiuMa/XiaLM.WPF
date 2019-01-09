using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaLM.Log.Help;
using XiaLM.Log.Model;

namespace XiaLM.Log
{
    /// <summary>
    /// 日志通过UDP输出
    /// </summary>
    public class LogOutPut
    {
        /// <summary>
        /// 是否允许写日志
        /// </summary>
        public bool CanWriteLog;
        /// <summary>
        /// 是否允许UDP发送日志
        /// </summary>
        public bool CanSendLog;
        private UdpClient client;
        /// <summary>
        /// UDP接收端ip+port
        /// </summary>
        private IPEndPoint serverPoint; 
        private static LogOutPut instance;
        private static readonly object lockObj = new object();

        public static LogOutPut GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new LogOutPut();
                    }
                }
            }
            return instance;
        }

        public LogOutPut()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "LogConfig.xml";
            if (!File.Exists(configPath)) throw new Exception("未找到配置文件！");
            Config config = XmlSerializeHelper.LoadXmlToObject<Config>(configPath);
            if (config == null) throw new Exception("配置文件格式不正确！");
            CanWriteLog = config.CanWriteLog;
            CanSendLog = config.CanSendLog;
            if (client == null) client = new UdpClient();
            serverPoint = new IPEndPoint(IPAddress.Parse(config.UdpServer.Ip), config.UdpServer.Port);
        }

        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        /// <param name="msg"></param>
        public void Send(OutMsg outMsg)
        {
            if (outMsg == null) return;
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(outMsg));
            client?.SendAsync(data, data.Length, serverPoint);
        }
    }

    /// <summary>
    /// UDP输出消息实体
    /// </summary>
    public class OutMsg
    {
        public string Client { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string Time { get; set; }
    }
}
