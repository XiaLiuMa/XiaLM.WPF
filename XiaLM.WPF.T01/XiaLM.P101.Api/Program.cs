using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Log;
using XiaLM.Owin;

namespace XiaLM.P101.Api
{
    class Program
    {
        static OwinRealize owin;
        static void Main(string[] args)
        {
            MainRealize.GetInstance().InitConfig();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "XiaLM.P101.Api");
            bool Running = !mutex.WaitOne(0, false);
            if (!Running)
            {
                try
                {
                    List<HttpRoute> routes = new List<HttpRoute>();
                    routes.Add(new HttpRoute()
                    {
                        RouteTemplate = @"Api/{controller}/{action}/{id}"
                    });
                    owin = new OwinRealize(@"http://127.0.0.1:10012", new OwinStart()
                    {
                        IsOpenSignalR = false,
                        IsCorss = true,
                        FileServerOptions = null,
                        Routes = routes,
                        ResultToJson = true,
                        IsOpenWebApi = true
                    });
                    owin.Start();
                    while (true) Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                    Environment.Exit(0);
                }
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error(e.ToString());
            Environment.Exit(0);
        }
    }
}
