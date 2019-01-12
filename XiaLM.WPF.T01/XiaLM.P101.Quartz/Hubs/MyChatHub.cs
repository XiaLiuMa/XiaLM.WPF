using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XiaLM.P101.Quartz.Entities;
using XiaLM.P101.Quartz.Jobs;
using XiaLM.P101.Quartz.Scheduler;

namespace XiaLM.P101.Quartz.Hubs
{
    public class MyChatHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// 开始跟随
        /// </summary>
        /// <param name="isWhite">是否跟随白名单</param>
        /// <returns></returns>
        public async Task StartFollow(bool isWhite)
        {
            
        }

        /// <summary>
        /// 反馈跟随状态
        /// </summary>
        /// <param name="state"></param>
        public async Task FeedBackFollowState(string state)
        {
            await this.Clients.All.SendAsync("FeedBackFollowState", state);
        }















        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IActionResult ExecuteJob(int id)
        {
            var db = DataHelper.GetInstance();
            var schedule = db.Queryable<Schedule>().InSingle(id);

            var scheduleEntity = DataMapper.MapperToModel(new ScheduleEntity(), schedule);
            //给IJob设置参数
            scheduleEntity.Agrs = new Dictionary<string, object> { { "orderId", id } };
            ScheduleManage.Instance.AddScheduleList(scheduleEntity);
            // 运行任务调度
            BaseQuartzNetResult result;
            if (schedule.TriggerType == 0)
            {
                result = SchedulerCenter.Instance.RunScheduleJob<ScheduleManage, SubmitJobTask>(schedule.JobGroup, schedule.JobName).Result;
            }
            else
            {
                result = SchedulerCenter.Instance.RunScheduleJob<ScheduleManage>(schedule.JobGroup, schedule.JobName).Result;
            }
            Console.Out.WriteLineAsync("任务执行状态：" + result.Msg);
            if (result.Code == 1000)
            {
                var t10 = db.Updateable<Schedule>().UpdateColumns(it => new Schedule()
                {
                    JobStatus = 1,
                    UpdateTime = DateTime.Now
                })
                .Where(it => it.JobId == scheduleEntity.JobId).ExecuteCommand();
                OperatelogService.AddLog(new OperateLog
                {
                    TableName = "Schedule",
                    Describe = "执行任务" + schedule.JobName + "成功",
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            return base.Json(new BaseResult { Code = result.Code == 1000 ? MsgCode.Success : MsgCode.IsFail, Msg = result.Msg });
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <returns></returns>
        public IActionResult StopScheduleJob(int id)
        {
            var db = DataHelper.GetInstance();
            //根据任务编号获取任务详情
            var schedule = db.Queryable<Schedule>().InSingle(id);
            //停止指定任务
            var result = SchedulerCenter.Instance.StopScheduleJob<ScheduleManage>(schedule.JobGroup, schedule.JobName);
            if (result.Result.Code == 1000)
            {
                db.Updateable<Schedule>()
                  .UpdateColumns(it => new Schedule() { JobStatus = 0, UpdateTime = DateTime.Now })
                  .Where(it => it.JobId == id).ExecuteCommand();
                OperatelogService.AddLog(new OperateLog
                {
                    TableName = "Schedule",
                    Describe = "停止任务" + schedule.JobName + "成功",
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            return base.Json(new BaseResult { Code = result.Result.Code == 1000 ? MsgCode.Success : MsgCode.IsFail, Msg = result.Result.Msg });
        }

    }
}
