using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using XiaLM.P101.Quartz.Scheduler.Models;

namespace XiaLM.P101.Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            Schedule schedule = new Schedule();
            string str1 = Assembly.GetExecutingAssembly().FullName.Split(',')[0];   //XiaLM.P101.Quartz
            string str2 = schedule.GetType().Namespace + "." + schedule.GetType().Name; //XiaLM.P101.Quartz.Scheduler.Models.Schedule

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "XiaLM.P101.Quartz");
            bool Running = !mutex.WaitOne(0, false);
            if (!Running)
            {
                var host = new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel()
                    .UseUrls("http://localhost:5000")
                    .UseStartup<Startup>()
                    .Build();
                host.Run();

                while (true) Console.ReadKey();
            }
        }

        /// <summary>
        /// 这行代码是必须要的，否则数据库无法迁移
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
