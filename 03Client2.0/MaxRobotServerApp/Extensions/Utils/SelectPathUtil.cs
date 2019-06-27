using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace MaxRobotServerApp.Extensions.Utils
{
    public class SelectPathUtil
    {
        /// <summary>
        /// 选择文件
        /// </summary>
        public static void OpenSelectFileDialog(ref string path, string filter= "All|*.*")
        {
            var dlg = new OpenFileDialog { Filter = filter };
            var res = dlg.ShowDialog();
            if (res != true) return;
            path = dlg.FileName;
        }
    }
}
