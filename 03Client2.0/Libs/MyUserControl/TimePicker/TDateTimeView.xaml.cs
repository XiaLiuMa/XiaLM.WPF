using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MyUserControl.TimePicker
{
    [System.ComponentModel.DesignTimeVisible(false)]//在工具箱中 隐藏该窗体
    /// <summary>
    /// TDateTime.xaml 的交互逻辑
    /// </summary>
    public partial class TDateTimeView : UserControl
    {
        public TDateTimeView()
        {
            InitializeComponent();
            InitHourMinuteSecond();
        }

        #region 时分秒区域
        /// <summary>
        /// 初始化时分秒
        /// </summary>
        private void InitHourMinuteSecond()
        {
            this.textbox_minute.Background = Brushes.White;
            this.textbox_second.Background = Brushes.White;
            this.textbox_hour.Background = Brushes.White;
            InitParameters();
        }

        /// <summary>
        /// 初始化参数设置
        /// </summary>
        private void InitParameters()
        {
            string strt = System.DateTime.Now.ToString("HH:mm:ss");
            this.textbox_hour.Text = strt.Split(':')[0];
            this.textbox_minute.Text = strt.Split(':')[1];
            this.textbox_second.Text = strt.Split(':')[2];
        }

        /// <summary>
        /// 更改选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Textbox_hour_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                switch (tb.Name)
                {
                    case "textbox_hour":
                        tb.Background = Brushes.Gray;
                        this.textbox_minute.Background = Brushes.White;
                        this.textbox_second.Background = Brushes.White;
                        break;
                    case "textbox_minute":
                        tb.Background = Brushes.Gray;
                        this.textbox_hour.Background = Brushes.White;
                        this.textbox_second.Background = Brushes.White;
                        break;
                    case "textbox_second":
                        tb.Background = Brushes.Gray;
                        this.textbox_hour.Background = Brushes.White;
                        this.textbox_minute.Background = Brushes.White;
                        break;
                }
            }
        }

        /// <summary>
        /// 向上加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_up_Click(object sender, RoutedEventArgs e)
        {
            if (this.textbox_hour.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_hour.Text);
                temp++;
                if (temp >= 24)
                {
                    temp = 0;
                }
                this.textbox_hour.Text = temp.ToString();
            }
            else if (this.textbox_minute.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_minute.Text);
                temp++;
                if (temp >= 60)
                {
                    temp = 0;
                }
                this.textbox_minute.Text = temp.ToString();
            }
            else if (this.textbox_second.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_second.Text);
                temp++;
                if (temp >= 60)
                {
                    temp = 0;
                }
                this.textbox_second.Text = temp.ToString();
            }
        }

        /// <summary>
        /// 向下减
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_down_Click(object sender, RoutedEventArgs e)
        {
            if (this.textbox_hour.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_hour.Text);
                temp--;
                if (temp < 0)
                {
                    temp = 23;
                }
                this.textbox_hour.Text = temp.ToString();
            }
            else if (this.textbox_minute.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_minute.Text);
                temp--;
                if (temp < 0)
                {
                    temp = 59;
                }
                this.textbox_minute.Text = temp.ToString();
            }
            else if (this.textbox_second.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_second.Text);
                temp--;
                if (temp < 0)
                {
                    temp = 59;
                }
                this.textbox_second.Text = temp.ToString();
            }
        }

        /// <summary>
        /// 数字标准化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numtextboxchanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if ((this.isNum(tb.Text) == false) || (tb.Text.Length > 2))
                {
                    tb.Text = "0";
                    MessageBox.Show("请输入正确的时间！", "警告！");
                    return;
                }
            }
        }

        /// <summary>
        /// 判断是否为数字，是--->true，否--->false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isNum(string str)
        {
            bool ret = true;
            foreach (char c in str)
            {
                if ((c < 48) || (c > 57))
                {
                    return false;
                }
            }

            return ret;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 当前按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNow_Click(object sender, RoutedEventArgs e)
        {
            string strt = System.DateTime.Now.ToString("HH:mm:ss");
            this.textbox_hour.Text = strt.Split(':')[0];
            this.textbox_minute.Text = strt.Split(':')[1];
            this.textbox_second.Text = strt.Split(':')[2];
        }

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DateTime? dt = new DateTime?();

            if (calDate.SelectedDate == null)
            {
                dt = DateTime.Now.Date;
            }
            else
            {
                dt = calDate.SelectedDate;
            }

            DateTime dtCal = Convert.ToDateTime(dt);

            string timeStr = "00:00:00";
            timeStr = $"{textbox_hour.Text.Trim()}:{textbox_minute.Text.Trim()}:{textbox_second.Text.Trim()}";

            string dateStr;
            dateStr = dtCal.ToString("yyyy-MM-dd");

            string dateTimeStr;
            dateTimeStr = dateStr + " " + timeStr;

            string str1 = string.Empty; ;
            str1 = dateTimeStr;
            OnDateTimeContent(str1);
        }

        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iBtnCloseView_Click(object sender, RoutedEventArgs e)
        {
            OnDateTimeContent(string.Empty);
        }

        /// <summary>
        /// 解除calendar点击后 影响其他按钮首次点击无效的问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calDate_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }


        #endregion

        #region Action交互

        /// <summary>
        /// 时间确定后的传递事件
        /// </summary>
        public Action<string> DateTimeOK;

        /// <summary>
        /// 时间确定后传递的时间内容
        /// </summary>
        /// <param name="dateTimeStr"></param>
        protected void OnDateTimeContent(string dateTimeStr)
        {
            if (DateTimeOK != null) DateTimeOK(dateTimeStr);
        }

        #endregion

    }
}
