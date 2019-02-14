using Nancy;
using System;
using System.Collections.Generic;
using System.Text;
using XiaLM.P101.Quartz.Db.IManaments;
using XiaLM.P101.Quartz.App.Hubs;
using XiaLM.P101.Quartz.Scheduler;
using XiaLM.P101.Quartz.Scheduler.Models;
using AutoMapper;
using XiaLM.P101.Quartz.Db.Entities;

namespace XiaLM.P101.Quartz.App.Modules
{
    public class HomeModule : NancyModule
    {
        private readonly IScheduleManament manament;

        public HomeModule(IScheduleManament _manament) : this()
        {
            manament = _manament;
        }

        public HomeModule()
        {
            Get("/hello", p => View[@"hello.html"]);  //返回视图
            Get("/test", p =>
            {
                var model = DateTime.Now.ToString();
                return View[@"hello.html", model];
            });  //带参返回视图

            Get("/api", args => "Hello World, it's Nancy on .NET Core");
            Get("/", p => View["App/Views/Home/home.html"]);  //返回视图

            Post("/Home/PushAlarm", p =>    //报警推送
            {
                var alarm = (string)Request.Form.Alarm;
                return PushAlarm(alarm);
            });

            Post("/Home/SelectSchedules", p =>  //查询数据库所有任务计划
            {
                var list = manament.GetAllList();
                return Mapper.Map<List<ScheduleEntity>, List<Schedule>>(list);
            });

            Post("/Home/AddSchedule", p =>  //新增数据库任务计划
            {
                //Schedule obj1 = (Schedule)Request.Form;
                Schedule obj = new Schedule()
                {
                    Id = Guid.NewGuid(),
                    JobGroup = "group2",
                    JobName = "name2",
                    Cron = "",
                    AssemblyName = "XiaLM.P101.Quartz",
                    ClassName = "XiaLM.P101.Quartz.Scheduler.Jobs.HelloJob",
                    RunTimes = 5,
                    BeginTime = DateTime.Now.AddSeconds(2),
                    EndTime = null,
                    IntervalSecond = 5,
                    Url = "123",
                    Remark = "4564654",
                    NextTime = null,
                    Status = EnumType.JobStatus.Paused,
                    Agrs = null
                };
                return manament.Insert(Mapper.Map<Schedule, ScheduleEntity>(obj));
            });

            Post("/Home/DeleteSchedule", p =>   //删除数据库任务计划
            {
                var id = (Guid)Request.Form.Id;
                manament.Delete(id);
                return true;
            });

            Post("/Home/UpdateSchedule", p =>  //修改数据库任务计划
            {
                Schedule obj = (Schedule)Request.Form;
                return manament.Update(Mapper.Map<Schedule, ScheduleEntity>(obj));
            });

            Post("/Home/ExecuteJob", p =>   //执行计划
            {
                return ExecuteJob((Guid)Request.Form.Id);
            });

            Post("/Home/StopJob", p =>  //暂停计划
            {
                return StopJob((Guid)Request.Form.Id);
            });

            Post("/Home/ResumeJob", p =>    //恢复运行(被暂停的)计划
            {
                return ResumeJob((Guid)Request.Form.Id);
            });

            Post("/Home/StopAndDeleteJob", p => //停止并删除计划
            {
                return StopAndDeleteJob((Guid)Request.Form.Id);
            });

        }

        /// <summary>
        /// 报警推送
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        private bool PushAlarm(string alarm)
        {
            HomeHub.GetInstance().FeedScheduleAlarm(alarm);
            return true;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        private bool ExecuteJob(Guid jobId)
        {
            var result = SchedulerCenter.GetInstance().RunScheduleJob(Mapper.Map<ScheduleEntity, Schedule>(manament.Get(jobId)));
            return true;
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        private bool StopJob(Guid jobId)
        {
            var result = SchedulerCenter.GetInstance().StopScheduleJob(jobId);
            return true;
        }

        /// <summary>
        /// 恢复运行(被暂停的)计划
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        private bool ResumeJob(Guid jobId)
        {
            SchedulerCenter.GetInstance().ResumeJob(jobId);
            return true;
        }

        /// <summary>
        /// 停止并删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        private bool StopAndDeleteJob(Guid jobId)
        {
            var result = SchedulerCenter.GetInstance().StopScheduleJob(jobId, true);
            return true;
        }
    }
}
