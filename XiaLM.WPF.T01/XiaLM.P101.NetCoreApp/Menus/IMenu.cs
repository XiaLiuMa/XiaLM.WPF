using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XiaLM.P101.NetCoreApp.Menus
{
    public interface IMenu
    {
        void Send(string message);
    }
}
