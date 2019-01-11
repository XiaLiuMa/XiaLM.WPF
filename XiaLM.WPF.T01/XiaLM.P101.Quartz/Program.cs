using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace XiaLM.P101.Quartz
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestTask().StartTestAsync();

            Console.ReadKey();


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:8000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
