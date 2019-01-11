using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XiaLM.P101.NetCoreApp.Menus;

namespace XiaLM.P101.NetCoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:8000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();



            ////配置MEF容器
            //var configuration = new ContainerConfiguration().WithAssembly(typeof(Program).GetTypeInfo().Assembly);
            //using (var container = configuration.CreateContainer())
            //{
            //    //依据相应接口获取导出的具体类
            //    IMenu menu = container.GetExport<IMenu>();
            //    menu.Send("Hello MEF2");
            //}

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
