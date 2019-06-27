using System;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Input;

namespace MyUserControl.TimePicker
{

    [ToolboxBitmap(typeof(DateTimePicker), "DateTimePicker.bmp")]  
    /// <summary>
    /// DateTimePicker.xaml 的交互逻辑
    /// </summary>    
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 日期时间
        /// </summary>
        public string UDateTime
        {
            get
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(textBox1.Text.Trim()); //try来保证输入的是时间格式
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception)
                {
                    if (GetIsStartTime(this))   //开始时间(默认为7天前)
                    {
                        textBox1.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else //结束时间(当前时间)
                    {
                        textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// DateTimePicker 窗体登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (GetIsStartTime(this))   //开始时间(默认为7天前)
            {
                textBox1.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else //结束时间(当前时间)
            {
                textBox1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 日历图标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IconButton1_Click(object sender, RoutedEventArgs e)
        {    
            if (popChioce.IsOpen == true) popChioce.IsOpen = false;
            TDateTimeView dtView = new TDateTimeView();// TDateTimeView  构造函数传入日期时间
            dtView.DateTimeOK += (dateTimeStr) => //TDateTimeView 日期时间确定事件
            {
                if (!string.IsNullOrEmpty(dateTimeStr))
                {
                    textBox1.Text = dateTimeStr;
                }
                popChioce.IsOpen = false;//TDateTimeView 所在pop  关闭
            };

            popChioce.Child = dtView;
            popChioce.IsOpen = true;
        }

        #region 控件属性
        /// <summary>
        /// 是否是开始时间
        /// </summary>
        public static readonly DependencyProperty IsStartTimeProperty =
            DependencyProperty.RegisterAttached("IsStartTime", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false));
        public static bool GetIsStartTime(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsStartTimeProperty);
        }
        public static void SetIsStartTime(DependencyObject obj, bool value)
        {
            obj.SetValue(IsStartTimeProperty, value);
        }
        #endregion
    }
}
