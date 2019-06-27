using MyCustomControl;
using System;
using System.Windows;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// RobotInfoFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class RobotInfoFlyout : CBaseFlyout
    {
        private string Tag { get; set; }

        /// <summary>
        /// 提交成功事件
        /// </summary>
        public event Action<string, string> ConfirmSuccess = (p1, p2) => { };

        public RobotInfoFlyout(string tag, string name)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Tag = tag;
            this.name.Text = name;
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.name.Text.Trim())) return;  //未填名称
            try
            {
                ConfirmSuccess(Tag, this.name.Text.Trim());
                
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
