﻿using Quartz;
using System;
using System.Threading.Tasks;

namespace XiaLM.P101.Quartz.Scheduler.Jobs
{
    public class SubmitJobTask : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //var manage = new ScheduleManage();
            //var schedule = new ScheduleEntity
            //{
            //    JobGroup = context.JobDetail.Key.Group,
            //    JobName = context.JobDetail.Key.Name,
            //    RunStatus = EnumType.JobRunStatus.执行中
            //};
            //manage.UpdateScheduleRunStatus(schedule);
            //var model = manage.GetScheduleModel(context.JobDetail.Key.Group, context.JobDetail.Key.Name);
            //await Console.Out.WriteLineAsync(string.Format("试一试:任务分组：{0}任务名称：{1}任务状态：{2}", schedule.JobGroup, schedule.JobName, schedule.RunStatus));
        }
    }
}
