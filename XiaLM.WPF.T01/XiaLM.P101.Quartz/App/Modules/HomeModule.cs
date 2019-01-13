using Nancy;
using System;
using System.Collections.Generic;
using System.Text;
using XiaLM.P101.Quartz.Db.IManaments;
using XiaLM.P101.Quartz.Hubs;

namespace XiaLM.P101.Quartz.App.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/api", args => "Hello World, it's Nancy on .NET Core");
            Get("/", p => View["chat.html"]);  //返回视图

            Post("/Home/ScheduleAlarm", p => 
            {
                var userName = (string)Request.Form.UserName;
                return ScheduleAlarm(userName);
            });
        }

        private bool ScheduleAlarm(string userName)
        {
            return HelloHub.GetInstance().FeedScheduleAlarm(userName);
        }
    }
}
