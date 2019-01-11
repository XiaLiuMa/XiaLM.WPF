using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace XiaLM.P101.NetCoreApp.Menus
{
    [Export(typeof(IMenu))]
    public class SystemManagement : IMenu
    {
        public void Send(string message)
        {
            Console.WriteLine($"Email：{message}");
        }
    }
}
