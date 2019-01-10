using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.P101.Installer.Message.Event_Args
{
    /// <summary>
    /// 安装步骤跳转事件
    /// </summary>
    public class InstallStepJumpEventArgs : EventArgs
    {
        public int Step { get; }
        public InstallStepJumpEventArgs(int _Step)
        {
            this.Step = _Step;
        }
    }
}
