using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
namespace XiaLM.P101.Quartz.Db
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            //using (var context = new BaseDBContext(serviceProvider.GetRequiredService<DbContextOptions<BaseDBContext>>()))
            using (var context = BaseDBContext.GetInstance())
            {
                if (context.Schedules.Any()) return;   // 已经初始化过数据，直接返回

                //增加一个计划
                context.Schedules.Add(
                   new Entities.ScheduleEntity
                   {
                       JobGroup = "group1",
                       JobName = "name1",
                       Cron = "0 0/5 * * * ?",
                       AssemblyName = "XiaLM.P101.Quartz",
                       ClassName = "XiaLM.P101.Quartz.Scheduler.Jobs.HelloJob",
                       RunTimes = 10,
                       BeginTime = DateTime.Now.AddSeconds(2),
                       EndTime = null,
                       IntervalSecond = 5,
                       Url = "123",
                       Remark = "4564654",
                       CreateTime = DateTime.Now,
                       TriggerType = 0,
                       UpdateTime = null,
                       Valid = 1
                   }
                );
                context.SaveChanges();
            }
        }
    }
}
