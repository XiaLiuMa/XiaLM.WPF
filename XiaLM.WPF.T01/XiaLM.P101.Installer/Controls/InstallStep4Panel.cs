using System;
using System.Windows.Forms;
using XiaLM.P101.Installer.Message;
using XiaLM.P101.Installer.Message.Event_Args;
using XiaLM.Utility;

namespace XiaLM.P101.Installer.Controls
{
    public partial class InstallStep4Panel : UserControl
    {
        public InstallStep4Panel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            MessageRealize.GetInstance().SendMsg(new InstallStepJumpEventArgs(3)).Employ();
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            MessageRealize.GetInstance().SendMsg(new InstallStepJumpEventArgs(5)).Employ();
        }
    }
}