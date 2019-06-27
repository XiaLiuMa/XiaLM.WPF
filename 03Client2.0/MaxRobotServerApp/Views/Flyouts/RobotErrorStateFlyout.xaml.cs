using MyCustomControl;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// RobotErrorStateFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class RobotErrorStateFlyout : CBaseFlyout
    {
        public RobotErrorStateFlyout(List<string> errors)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit(errors);
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInit(List<string> errors)
        {
            this.errorGrid.Children.Clear();
            if (errors == null || errors.Count <= 0) return;
            for (int i = 0; i < errors.Count; i++)
            {
                this.errorGrid.RowDefinitions.Add(new RowDefinition()); //增加行
                Label lb = new Label();
                lb.Content = errors[i];
                lb.FontSize = 12;
                lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                lb.VerticalContentAlignment = VerticalAlignment.Center;

                this.errorGrid.Children.Add(lb); //添加到Grid控件
                lb.SetValue(Grid.RowProperty, i); //设置控件所在Grid的行
                lb.SetValue(Grid.ColumnProperty, 0); //设置控件所在Grid的列
            }
        }
    }
}
