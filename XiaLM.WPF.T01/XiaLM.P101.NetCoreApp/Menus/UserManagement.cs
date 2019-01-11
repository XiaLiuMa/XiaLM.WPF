using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace XiaLM.P101.NetCoreApp.Menus
{
    [Export(typeof(IMenu))]
    public class UserManagement : IMenu
    {
        public void Send(string message)
        {
            Console.WriteLine($"Email：{message}");
        }
    }
}
