using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyCustomControl
{
    /// <summary>
    /// WPF弹窗口基类
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    public class CBaseFlyout : Window
    {
        public CBaseFlyout()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeStyle();
            this.Loaded += delegate
            {
                InitializeEvent(base.Title);
            };
        }

        private void InitializeEvent(string title = "TeryWindow")
        {
            ControlTemplate TeryWindowTemplate = (ControlTemplate)Application.Current.Resources["TeryWindowControlTemplate"];

            TextBlock winTitle = (TextBlock)TeryWindowTemplate.FindName("winTitle", this);
            winTitle.Text = title;

            Image closeBtn = (Image)TeryWindowTemplate.FindName("btnClose", this);
            closeBtn.MouseDown += delegate
            {
                this.Close();
            };

            Border borderTitle = (Border)TeryWindowTemplate.FindName("borderTitle", this);
            borderTitle.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            borderTitle.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
            {
                
            };
        }


        private void InitializeStyle()
        {
            //xaml设置样式无效。。。故代码设置
            this.WindowStyle = WindowStyle.None;
            this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            this.ResizeMode = ResizeMode.NoResize;
            this.AllowsTransparency = true;


            this.Style = (Style)Application.Current.Resources["TeryWindowStyle"];
        }
    }
}
