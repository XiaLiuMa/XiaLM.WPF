using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiaLM.P101.Installer.Message;
using XiaLM.P101.Installer.Message.Event_Args;
using XiaLM.Utility;

namespace XiaLM.P101.Installer.Controls
{
    public partial class InstallStep5Panel : UserControl
    {
        public InstallStep5Panel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 完成安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            MessageRealize.GetInstance().SendMsg(new InstallStepJumpEventArgs(-1)).Employ();
        }
    }
}
