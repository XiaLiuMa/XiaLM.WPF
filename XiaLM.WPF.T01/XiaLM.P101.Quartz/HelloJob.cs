using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.P101.Quartz
{
    public class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.Out.WriteLineAsync("Greetings from HelloJob!"); return Task.CompletedTask;
        }
    }
}
