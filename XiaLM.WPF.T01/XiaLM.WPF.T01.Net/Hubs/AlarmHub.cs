using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Owin.SignalR;
using XiaLM.WPF.T01.Net.Models;

namespace XiaLM.WPF.T01.Net.Hubs
{
    /// <summary>
    /// 报警消息推送
    /// </summary>
    public class AlarmHub : Microsoft.AspNet.SignalR.Hub
    {
        private static readonly object lockObj = new object();
        private static AlarmHub instance;
        public static AlarmHub GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new AlarmHub();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 推送报警消息
        /// </summary>
        public void PushAlarmInfo(AlarmInfo alarm)
        {
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<AlarmHub>();
                hub.Clients.All.RobotStateMonitor(alarm);
            }
            catch (Exception)
            {
            }
        }
    }
}
