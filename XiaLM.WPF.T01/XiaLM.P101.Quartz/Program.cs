using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace XiaLM.P101.Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "XiaLM.P101.Quartz");
            bool Running = !mutex.WaitOne(0, false);
            if (!Running)
            {
                //new TestTask().StartTestAsync();
                var host = new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel()
                    .UseUrls("http://localhost:5000")
                    //.UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();
                host.Run();

                while (true) Console.ReadKey();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
