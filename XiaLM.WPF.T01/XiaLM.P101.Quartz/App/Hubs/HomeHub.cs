using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XiaLM.P101.Quartz.Db.IManaments;
using XiaLM.P101.Quartz.Scheduler;
using XiaLM.P101.Quartz.Scheduler.Jobs;
using XiaLM.P101.Quartz.Scheduler.Models;

namespace XiaLM.P101.Quartz.App.Hubs
{
    public class HomeHub : Hub
    {
        //private IScheduleManament _manament;
        //public IScheduleManament manament
        //{
        //    get { return _manament; }
        //    set { _manament = value; }
        //}

        private readonly static object objLock = new object();
        private static HomeHub instance = null;
        public static HomeHub GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new HomeHub();
                    }
                }
            }
            return instance;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 查询所有任务计划
        /// </summary>
        /// <returns></returns>
        public object GetAllObjs()
        {
            //return manament.GetAllList();
            return null;
        }

        /// <summary>
        /// 反馈任务调度报警
        /// </summary>
        /// <param name="state"></param>
        public bool FeedScheduleAlarm(string state)
        {
            if (this.Clients != null) this.Clients.All.SendAsync("FeedScheduleAlarm", state);
            return true;
        }

    }
}
