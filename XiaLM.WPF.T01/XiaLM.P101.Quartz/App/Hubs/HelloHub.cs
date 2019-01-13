using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XiaLM.P101.Quartz.App.Jobs;
using XiaLM.P101.Quartz.Db.IManaments;
using XiaLM.P101.Quartz.Db.Manaments;
using XiaLM.P101.Quartz.Jobs;
using XiaLM.P101.Quartz.Scheduler;

namespace XiaLM.P101.Quartz.Hubs
{
    public class HelloHub : Hub
    {
        private IScheduleManament _manament;
        public IScheduleManament manament
        {
            get { return _manament; }
            set { _manament = value; }
        }

        private readonly static object objLock = new object();
        private static HelloHub instance = null;
        public static HelloHub GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new HelloHub();
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
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public object GetAllObjs()
        {
            return manament.GetAllList();
        }

        /// <summary>
        /// 启动任务调度
        /// </summary>
        /// <param name="guid">任务调度id</param>
        /// <returns></returns>
        public async Task StartSchedule(Guid guid)
        {
            var schedule = manament.Get(guid);
            var scheduleEntity = Mapper.Map(schedule,new ScheduleEntity());
            scheduleEntity.Agrs = new Dictionary<string, object> { { "orderId", guid } }; //给IJob设置参数
            ScheduleManage.Instance.AddScheduleList(scheduleEntity);
            var result = await SchedulerCenter.Instance.RunScheduleJob<ScheduleManage, SubmitJobTask>(scheduleEntity.JobGroup, scheduleEntity.JobName);
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <param name="guid">任务调度id</param>
        /// <returns></returns>
        public async Task StopSchedule(Guid guid)
        {
            var schedule = manament.Get(guid);
            var result = SchedulerCenter.Instance.StopScheduleJob<ScheduleManage>(schedule.JobGroup, schedule.JobName);
        }

        /// <summary>
        /// 反馈任务调度报警
        /// </summary>
        /// <param name="state"></param>
        public bool FeedScheduleAlarm(string state)
        {
            this.Clients.All.SendAsync("FeedScheduleAlarm", state);
            return true;
        }

    }
}
