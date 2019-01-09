using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.Log;
using XiaLM.Owin;
using XiaLM.Owin.FileSever;

namespace XiaLM.WPF.T01.Net
{
    class Program
    {
        static OwinRealize owin;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "XiaLM.WPF.T01.Net");
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
                    owin = new OwinRealize(@"http://127.0.0.1:10011", new OwinStart()
                    {
                        IsOpenSignalR = true,
                        IsCorss = true,
                        FileServerOptions = new FileServerOptions(null, AppDomain.CurrentDomain.BaseDirectory + @"WebFolders", true),
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
