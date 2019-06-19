using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyUserControl
{
    /// <summary>
    /// CustomBorder.xaml 的交互逻辑
    /// </summary>
    public partial class CustomBorder : UserControl
    {
        public CustomBorder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化边框的4个角
        /// </summary>
        private void InitBoder()
        {
            //this.line1.X1 = 0;
            //this.line1.Y1 = -5;
            //this.line1.X2 = 0;
            //this.line1.Y2 = 40;

            //this.line2.X1 = -5;
            //this.line2.Y1 = 0;
            //this.line2.X2 = 40;
            //this.line2.Y2 = 0;

            //this.line3.X1 = this.grid1.Width - 40;
            //this.line3.Y1 = 0;
            //this.line3.X2 = this.grid1.Width + 5;
            //this.line3.Y2 = 40;

            //this.line4.X1 = this.grid1.Width;
            //this.line4.Y1 = -5;
            //this.line4.X2 = -this.grid1.Width;
            //this.line4.Y2 = 40;

            //this.line5.X1 = this.grid1.Width;
            //this.line5.Y1 = this.grid1.Height - 40;
            //this.line5.X2 = this.grid1.Width;
            //this.line5.Y2 = this.grid1.Height + 5;

            //this.line6.X1 = this.grid1.Width - 40;
            //this.line6.Y1 = this.grid1.Height;
            //this.line6.X2 = this.grid1.Width + 5;
            //this.line6.Y2 = this.grid1.Height;

            //this.line7.X1 = -5;
            //this.line7.Y1 = this.grid1.Height; ;
            //this.line7.X2 = 40;
            //this.line7.Y2 = this.grid1.Height; ;

            //this.line8.X1 = 0;
            //this.line8.Y1 = 0;
            //this.line8.X2 = this.grid1.Height - 40;
            //this.line8.Y2 = this.grid1.Width + 5;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitBoder();
        }

        //#region 依赖属性
        //public string ImagePath
        //{
        //    get { return (string)GetValue(ImagePathProperty); }
        //    set { SetValue(ImagePathProperty, value); }
        //}
        //public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register("ImagePath", typeof(string), typeof(CustomBorder),
        //    new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        //#endregion 依赖属性

        //#region 事件
        ///// <summary>
        ///// 图片资源修改时触发
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    try
        //    {
        //        Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + ex.StackTrace);
        //    }

        //}
        //#endregion 事件
    }
}
