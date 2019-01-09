using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Owin.SignalR
{
    /// <summary>
    /// 集线器，需要引用packages\Microsoft.AspNet.SignalR.Core.2.2.2\lib\net45\Microsoft.AspNet.SignalR.Core.dll
    /// </summary>
    public abstract class Hub : Microsoft.AspNet.SignalR.Hub
    {
    }
}
