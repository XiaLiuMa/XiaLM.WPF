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
    public partial class InstallStep1Panel : UserControl
    {
        public InstallStep1Panel()
        {
            InitializeComponent();
        }

        private void InstallStep1Panel_Load(object sender, EventArgs e)
        {
            if (!this.checkBox1.Checked) this.button1.Enabled = false;
        }

        private void checkBox1_AppearanceChanged(object sender, EventArgs e)
        {
            if (!this.checkBox1.Checked) this.button1.Enabled = false;
            if (this.checkBox1.Checked) this.button1.Enabled = true;
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            MessageRealize.GetInstance().SendMsg(new InstallStepJumpEventArgs(2)).Employ();
        }
    }
}
