using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using XiaLM.P101.Quartz.Common;
using XiaLM.P101.Quartz.Scheduler.Models;

namespace XiaLM.P101.Quartz.Scheduler
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    public class SchedulerCenter
    {
        public List<Schedule> ScheduleList; //任务计划列表
        private static SchedulerCenter instance;
        private readonly static object objLock = new object();
        public SchedulerCenter()
        {
            ScheduleList = new List<Schedule>();
        }

        public static SchedulerCenter GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new SchedulerCenter();
                    }
                }
            }
            return instance;
        }

        private Task<IScheduler> _scheduler;
        /// <summary>
        /// 任务调度器
        /// </summary>
        /// <returns></returns>
        private Task<IScheduler> Scheduler
        {
            get
            {
                if (this._scheduler != null)
                {
                    return this._scheduler;
                }
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                    #region 以下配置需要数据库表配合使用，表结构sql地址：https://github.com/quartznet/quartznet/tree/master/database/tables
                    //{ "quartz.jobStore.type","Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"},
                    //{ "quartz.jobStore.driverDelegateType","Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz"},
                    //{ "quartz.jobStore.tablePrefix","QRTZ_"},
                    //{ "quartz.jobStore.dataSource","myDS"},
                    //{ "quartz.dataSource.myDS.connectionString",AppSettingHelper.MysqlConnection},//连接字符串
                    //{ "quartz.dataSource.myDS.provider","MySql"},
                    //{ "quartz.jobStore.useProperties","true"} 
	                #endregion
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                return this._scheduler = factory.GetScheduler();    //从Factory中获取Scheduler实例
            }
        }

        /// <summary>
        /// 获取任务计划列表
        /// </summary>
        public List<Schedule> GetScheduleList()
        {
            return ScheduleList;
        }

        /// <summary>
        /// 开启任务调度器
        /// </summary>
        public async void StartScheduleAsync()
        {
            if (!this.Scheduler.Result.IsStarted) await this.Scheduler.Result.Start();
        }

        /// <summary>
        /// 停止任务调度器
        /// </summary>
        public async void StopScheduleAsync()
        {
            if (!this.Scheduler.Result.IsShutdown) await this.Scheduler.Result.Shutdown();
        }

        /// <summary>
        /// 运行指定的计划
        /// </summary>
        /// <param name="schedule">jobId</param>
        /// <returns></returns>
        public async Task<BaseQuartzResult> RunScheduleJob(Schedule schedule)
        {
            StartScheduleAsync();
            BaseQuartzResult result;
            var runResult = await Task.Factory.StartNew(async () =>
            {
                var result1 = new BaseQuartzResult();
                try
                {
                    var jobType = AssemblyHandler.GetClassType(schedule.AssemblyName, $"{schedule.AssemblyName}.{schedule.ClassName}");  //反射获取任务执行类
                    IJobDetail job = new JobDetailImpl(schedule.JobName, schedule.JobGroup, jobType);   // 定义这个工作，并将其绑定到我们的IJob实现类
                    ITrigger trigger;   // 创建触发器
                    if (!string.IsNullOrEmpty(schedule.Cron) && CronExpression.IsValidExpression(schedule.Cron))  //校验是否正确的执行周期表达式
                    {
                        trigger = CreateCronTrigger(schedule);
                    }
                    else
                    {
                        trigger = CreateSimpleTrigger(schedule);
                    }

                    await this.Scheduler.Result.ScheduleJob(job, trigger);  // 告诉Quartz使用我们的触发器来安排作业
                    result1.Code = 1000;
                }
                catch (Exception ex)
                {
                    result1.Code = 1001;
                    result1.Msg = ex.Message;
                }
                return result1;
            });

            if (runResult.Result.Code == 1000)
            {
                ScheduleList.Where(p => p.Id.Equals(schedule.Id)).FirstOrDefault().Status = EnumType.JobStatus.Running;
                await this.Scheduler.Result.ResumeJob(new JobKey(schedule.JobName, schedule.JobGroup));   //用给定的密钥恢复（取消暂停）IJobDetail
                result = new BaseQuartzResult
                {
                    Code = 1000,
                    Msg = "启动成功"
                };
            }
            else
            {
                result = new BaseQuartzResult
                {
                    Code = -1
                };
            }
            return result;
        }

        /// <summary>
        /// 停止指定的计划
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isDelete">停止并删除任务</param>
        /// <returns></returns>
        public BaseQuartzResult StopScheduleJob(Guid jobId, bool isDelete = false)
        {
            StartScheduleAsync();
            BaseQuartzResult result;
            try
            {
                Schedule schedule = ScheduleList.Where(p => p.Id.Equals(jobId)).FirstOrDefault(); //获取任务实例
                this.Scheduler.Result.PauseJob(new JobKey(schedule.JobName, schedule.JobGroup));
                if (isDelete) ScheduleList.Remove(schedule); //从列表移除
                result = new BaseQuartzResult
                {
                    Code = 1000,
                    Msg = "停止任务计划成功！"
                };
            }
            catch (Exception ex)
            {
                result = new BaseQuartzResult
                {
                    Code = -1,
                    Msg = "停止任务计划失败"
                };
            }
            return result;
        }

        /// <summary>
        /// 恢复运行(被暂停的)计划
        /// </summary>
        /// <param name="jobId">jobId</param>
        public async void ResumeJob(Guid jobId)
        {
            StartScheduleAsync();
            try
            {
                Schedule schedule = ScheduleList.Where(p => p.Id.Equals(jobId)).FirstOrDefault(); //获取任务实例
                var jk = new JobKey(schedule.JobName, schedule.JobGroup);
                if (await this.Scheduler.Result.CheckExists(jk))    //检查任务是否存在
                {
                    await this.Scheduler.Result.ResumeJob(jk);  //任务已经存在则暂停任务
                    await Console.Out.WriteLineAsync(string.Format("任务“{0}”恢复运行", schedule.JobName));
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(string.Format("恢复任务失败！{0}", ex));
            }
        }

        /// <summary>
        /// 创建类型Simple的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(Schedule m)
        {
            //作业触发器
            if (m.RunTimes > 0)
            {
                return TriggerBuilder.Create()
               .WithIdentity(m.JobName, m.JobGroup)
               .StartAt(m.BeginTime)//开始时间
               .EndAt(m.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(m.IntervalSecond)//执行时间间隔，单位秒
                   .WithRepeatCount(m.RunTimes))//执行次数、默认从0开始
                   .ForJob(m.JobName, m.JobGroup)//作业名称
               .Build();
            }
            else
            {
                return TriggerBuilder.Create()
               .WithIdentity(m.JobName, m.JobGroup)
               .StartAt(m.BeginTime)//开始时间
               .EndAt(m.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(m.IntervalSecond)//执行时间间隔，单位秒
                   .RepeatForever())//无限循环
                   .ForJob(m.JobName, m.JobGroup)//作业名称
               .Build();
            }

        }

        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(Schedule m)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(m.JobName, m.JobGroup)
                   .StartAt(m.BeginTime)//开始时间
                   .EndAt(m.EndTime)//结束时间
                   .WithCronSchedule(m.Cron)//指定cron表达式
                   .ForJob(m.JobName, m.JobGroup)//作业名称
                   .Build();
        }
    }
}
