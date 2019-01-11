using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XiaLM.P101.Installer.Controls
{
    /// <summary>
    /// 可设置边框样式的Panel
    /// </summary>
    public class PanelEx : Panel
    {
        private Color borderColor;  //边框颜色
        private float borderSize;    //边框宽度
        private Border3DStyle border3DStyle;
        private ToolStripStatusLabelBorderSides borderSide;
        private bool borderIsSingleMode;

        [DefaultValue(true), Description("指定边框是否为单色模式。false代表三维模式")]
        public bool BorderIsSingleMode
        {
            get { return borderIsSingleMode; }
            set
            {
                if (borderIsSingleMode == value) { return; }
                borderIsSingleMode = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Black"), Description("边框颜色。仅当边框为单色模式时有效")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor == value) { return; }
                borderColor = value;
                this.Invalidate();
            }
        }

        [DefaultValue(typeof(float), "1"), Description("边框粗细")]
        public float BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }

        [DefaultValue(Border3DStyle.Etched), Description("边框三维样式。仅当边框为三维模式时有效")]
        public Border3DStyle Border3DStyle
        {
            get { return border3DStyle; }
            set
            {
                if (border3DStyle == value) { return; }
                border3DStyle = value;
                this.Invalidate();
            }
        }

        //之所以不直接用Border3DSide是因为这货不被设计器支持，没法灵活选择位置组合
        [DefaultValue(ToolStripStatusLabelBorderSides.None), Description("边框位置。可自由启用各个方位的边框")]
        public ToolStripStatusLabelBorderSides BorderSide
        {
            get { return borderSide; }
            set
            {
                if (borderSide == value) { return; }
                borderSide = value;
                this.Invalidate();
            }
        }

        public PanelEx()
        {
            this.borderIsSingleMode = true;
            this.borderColor = Color.Black;
            this.borderSize = 1;
            this.border3DStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.borderSide = ToolStripStatusLabelBorderSides.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.BorderStyle != System.Windows.Forms.BorderStyle.None || BorderSide == ToolStripStatusLabelBorderSides.None) return;

            using (Graphics g = e.Graphics)
            {
                //单色模式
                if (this.BorderIsSingleMode)
                {
                    using (Pen pen = new Pen(BorderColor, BorderSize))
                    {
                        //若是四条边都启用，则直接画矩形
                        if (BorderSide == ToolStripStatusLabelBorderSides.All)
                        {
                            g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                        }
                        else //否则分别绘制线条
                        {
                            if ((BorderSide & ToolStripStatusLabelBorderSides.Top) == ToolStripStatusLabelBorderSides.Top)
                            {
                                g.DrawLine(pen, 0, 0, this.Width - 1, 0);
                            }

                            if ((BorderSide & ToolStripStatusLabelBorderSides.Right) == ToolStripStatusLabelBorderSides.Right)
                            {
                                g.DrawLine(pen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
                            }

                            if ((BorderSide & ToolStripStatusLabelBorderSides.Bottom) == ToolStripStatusLabelBorderSides.Bottom)
                            {
                                g.DrawLine(pen, 0, this.Height - 1, this.Width - 1, this.Height - 1);
                            }

                            if ((BorderSide & ToolStripStatusLabelBorderSides.Left) == ToolStripStatusLabelBorderSides.Left)
                            {
                                g.DrawLine(pen, 0, 0, 0, this.Height - 1);
                            }
                        }
                    }
                }
                else //三维模式
                {
                    ControlPaint.DrawBorder3D(g, this.ClientRectangle, this.Border3DStyle, (Border3DSide)BorderSide); //这儿要将ToolStripStatusLabelBorderSides转换为Border3DSide
                }
            }
        }
    }
}
