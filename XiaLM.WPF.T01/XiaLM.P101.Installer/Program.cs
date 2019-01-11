using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiaLM.P101.Installer.Forms;
using XiaLM.P101.Installer.Message;

namespace XiaLM.P101.Installer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            MainInit();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            Application.Run(InstallForm.GetInstance());
        }

        private static void MainInit()
        {
            MainRealize.GetInstance().InitConfig();
            ModuleComposePart.GetInstance().Init();
        }
    }
}
