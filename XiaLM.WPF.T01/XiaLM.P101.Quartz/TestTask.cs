using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using XiaLM.P101.Quartz.Jobs;

namespace XiaLM.P101.Quartz
{
    public class TestTask
    {
        public async Task StartTestAsync()
        {
            try
            {
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);// 从工厂中获取调度程序实例
                IScheduler scheduler = await factory.GetScheduler();
                await scheduler.Start();// 开启调度器           
                IJobDetail job = JobBuilder.Create<HelloJob>().WithIdentity("job1", "group1").Build();// 定义这个工作，并将其绑定到我们的IJob实现类
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                    .Build();// 触发作业立即运行，然后每10秒重复一次，无限循环              
                await scheduler.ScheduleJob(job, trigger);// 告诉Quartz使用我们的触发器来安排作业                
                await Task.Delay(TimeSpan.FromSeconds(60)); // 等待60秒               
                await scheduler.Shutdown();// 关闭调度程序





                #region 在特定的时间内建立触发器，无需重复
                ISimpleTrigger trigger1 = (ISimpleTrigger)TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartAt(DateTime.Now) //指定开始时间为当前系统时间
                    .ForJob("job1", "group1") //通过JobKey识别作业
                    .Build();
                #endregion

                #region 在特定的时间内建立触发器，无需重复
                ITrigger trigger2 = TriggerBuilder.Create()
                   .WithIdentity("trigger2", "group2")
                   .StartAt(DateTime.Now) // 指定开始时间
                   .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithRepeatCount(10)) // 请注意，重复10次将总共重复11次
                   .ForJob("job2", "group2") //通过JobKey识别作业                   
                   .Build();
                #endregion

                #region 构建一个触发器，将在未来五分钟内触发一次
                ITrigger trigger3 = trigger = (ISimpleTrigger)TriggerBuilder.Create()
                   .WithIdentity("trigger3", "group3")
                   .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Minute)) //使用DateBuilder将来创建一个时间日期
                   .ForJob("job3", "group3") //通过JobKey识别作业
                   .Build();
                #endregion

                #region 建立一个现在立即触发的触发器，然后每隔五分钟重复一次，直到22:00:00
                ITrigger trigger4 = trigger = TriggerBuilder.Create()
                           .WithIdentity("trigger4", "group4")
                           .StartNow()
                           .WithSimpleSchedule(x => x.WithIntervalInMinutes(5).RepeatForever())
                           .EndAt(DateBuilder.DateOf(22, 0, 0))
                           .Build();
                #endregion

                #region 建立一个触发器，在一个小时后触发，然后每2小时重复一次
                ITrigger trigger5 = TriggerBuilder.Create()
                   .WithIdentity("trigger5") // 由于未指定组，因此“trigger5”将位于默认分组中
                   .StartAt(DateBuilder.EvenHourDate(null)) // 获取下个一小时时间                 
                   .WithSimpleSchedule(x => x.WithIntervalInHours(2).RepeatForever())//执行间隔2小时
                   .Build();
                #endregion



            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}
