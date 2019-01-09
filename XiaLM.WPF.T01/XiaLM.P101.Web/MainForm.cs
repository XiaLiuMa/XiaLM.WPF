using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiaLM.P101.Web.Handlers;

namespace XiaLM.P101.Web
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            settings.CefCommandLineArgs.Add("enable-media-stream", "enable-media-stream");
            settings.IgnoreCertificateErrors = true;
            settings.LogSeverity = LogSeverity.Verbose;

            if (!Cef.Initialize(settings))
            {
                if (Environment.GetCommandLineArgs().Contains("--type=renderer"))
                {
                    Environment.Exit(0);
                }
            }

            var browser = new ChromiumWebBrowser(@"http://127.0.0.1:10011")
            {
                Dock = DockStyle.Fill
            };
            browser.MenuHandler = new MenuHandler();
            browser.LifeSpanHandler = new LifeSpanHandler();
            this.Controls.Add(browser);
        }
    }
}
