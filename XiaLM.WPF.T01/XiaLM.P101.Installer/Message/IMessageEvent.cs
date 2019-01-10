using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.P101.Installer.Message
{
    [InheritedExport]
    public interface IMessageEvent
    {
        /// <summary>
        /// EvenHandle文件夹下的Event文件夹下的类型
        /// </summary>
        Type EventArgsType { get; }
        /// <summary>
        /// 消息接收
        /// </summary>
        /// <param name="eventArgs"></param>
        Task OnMessage(EventArgs eventArgs);
    }
}
