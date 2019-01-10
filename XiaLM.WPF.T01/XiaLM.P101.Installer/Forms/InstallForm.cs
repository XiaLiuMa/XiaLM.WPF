using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XiaLM.Log;
using XiaLM.P101.Installer.Controls;
using XiaLM.P101.Installer.Message;
using XiaLM.P101.Installer.Message.Event_Args;

namespace XiaLM.P101.Installer.Forms
{
    public partial class InstallForm : Form
    {
        public int CurrentStep = 1;    //当前步骤

        private static InstallForm instance;
        private readonly static object objLock = new object();
        public static InstallForm GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new InstallForm();
                    }
                }
            }
            return instance;
        }

        public InstallForm()
        {
            InitializeComponent();
        }

        private void InstallForm_Load(object sender, EventArgs e)
        {
            JumpToStep(CurrentStep);
        }

        //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
        //public static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
        //public static extern bool ReleaseCapture();

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    const int WM_NCLBUTTONDOWN = 0x00A1;
        //    const int HTCAPTION = 2;

        //    base.OnMouseDown(e);
        //    if (e.Button == MouseButtons.Left)  // 按下的是鼠标左键                  
        //    {
        //        ReleaseCapture();   // 释放捕获                     
        //        SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);    // 拖动窗体                  
        //    }
        //}



        //private bool m_isMouseDown = false;
        //private Point m_mousePos = new Point();
        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //    m_mousePos = Cursor.Position;
        //    m_isMouseDown = true;
        //}
        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    base.OnMouseUp(e);
        //    m_isMouseDown = false;
        //}
        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    if (m_isMouseDown)
        //    {
        //        Point tempPos = Cursor.Position;
        //        this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
        //        m_mousePos = Cursor.Position;
        //    }
        //}


        private void DrawLine()
        {
            Pen p = new Pen(Color.Blue, 5);//设置笔的粗细为,颜色为蓝色
            Graphics g = this.CreateGraphics();
            p.DashStyle = DashStyle.Dot;//定义虚线的样式为点
            g.DrawLine(p, 10, 10, 200, 10);
            //自定义虚线
            p.DashPattern = new float[] { 2, 1 };//设置短划线和空白部分的数组
            g.DrawLine(p, 10, 20, 200, 20);
            g.Dispose();
            p.Dispose();
        }

        /// <summary>
        /// 跳转到指定步骤
        /// </summary>
        /// <param name="step"></param>
        public void JumpToStep(int step)
        {
            try
            {
                switch (step)
                {
                    case 1: //许可协议
                        {
                            CurrentStep = 1;
                            this.Invoke(new Action(() =>
                            {
                                this.labelStep1.ForeColor = Color.Red;
                                this.labelStep2.ForeColor = Color.Black;
                                this.pictureStep.Image = Properties.Resources.step1;
                                this.panelStep.Controls.Clear();
                                this.panelStep.Controls.Add(new InstallStep1Panel());
                            }));
                        }
                        break;
                    case 2: //环境检测
                        {
                            CurrentStep = 2;
                            this.Invoke(new Action(() =>
                            {
                                this.labelStep1.ForeColor = Color.Blue;
                                this.labelStep2.ForeColor = Color.Red;
                                this.labelStep3.ForeColor = Color.Black;
                                this.pictureStep.Image = Properties.Resources.step1;
                                this.panelStep.Controls.Clear();
                                this.panelStep.Controls.Add(new InstallStep2Panel());
                            }));
                        }
                        break;
                    case 3: //参数配置
                        {
                            CurrentStep = 3;
                            this.Invoke(new Action(() =>
                            {
                                this.labelStep2.ForeColor = Color.Blue;
                                this.labelStep3.ForeColor = Color.Red;
                                this.labelStep4.ForeColor = Color.Black;
                                this.pictureStep.Image = Properties.Resources.step1;
                                this.panelStep.Controls.Clear();
                                this.panelStep.Controls.Add(new InstallStep3Panel());
                            }));
                        }
                        break;
                    case 4: //正在安装
                        {
                            CurrentStep = 4;
                            this.Invoke(new Action(() =>
                            {
                                this.labelStep3.ForeColor = Color.Blue;
                                this.labelStep4.ForeColor = Color.Red;
                                this.labelStep5.ForeColor = Color.Black;
                                this.pictureStep.Image = Properties.Resources.step1;
                                this.panelStep.Controls.Clear();
                                this.panelStep.Controls.Add(new InstallStep4Panel());
                            }));
                        }
                        break;
                    case 5: //完成安装
                        {
                            CurrentStep = 5;
                            this.Invoke(new Action(() =>
                            {
                                this.labelStep4.ForeColor = Color.Blue;
                                this.labelStep5.ForeColor = Color.Red;
                                this.pictureStep.Image = Properties.Resources.step1;
                                this.panelStep.Controls.Clear();
                                this.panelStep.Controls.Add(new InstallStep5Panel());
                            }));
                        }
                        break;
                    case -1: //安装完成，关闭安装程序
                        {
                            this.Invoke(new Action(() =>
                            {
                                this.Close();
                            }));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

    }

    /// <summary>
    /// 安装步骤跳转事件
    /// </summary>
    public class InstallStepJumpEvent : IMessageEvent
    {
        public Type EventArgsType => typeof(InstallStepJumpEventArgs);

        public async Task OnMessage(EventArgs eventArgs)
        {
            var obj = eventArgs as InstallStepJumpEventArgs;
            if (obj != null)
            {
                InstallForm.GetInstance().JumpToStep(obj.Step);
            }
        }
    }
}
