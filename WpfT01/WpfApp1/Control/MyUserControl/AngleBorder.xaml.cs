using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyCustomControl
{
    /// <summary>
    /// AngleBorder.xaml 的交互逻辑
    /// </summary>
    public class AngleBorder : Grid
    {
        #region 依赖属性

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(AngleBorder), null);
        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(AngleBorder));
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set { SetValue(TitleProperty, value); }
        }

        #region 4个角，8条线
        public static readonly DependencyProperty Line1X1Property = DependencyProperty.Register("Line1X1", typeof(double), typeof(AngleBorder));
        public double Line1X1 { get { return (double)GetValue(Line1X1Property); } set { SetValue(Line1X1Property, value); } }
        public static readonly DependencyProperty Line1Y1Property = DependencyProperty.Register("Line1Y1", typeof(double), typeof(AngleBorder));
        public double Line1Y1 { get { return (double)GetValue(Line1Y1Property); } set { SetValue(Line1Y1Property, value); } }
        public static readonly DependencyProperty Line1X2Property = DependencyProperty.Register("Line1X2", typeof(double), typeof(AngleBorder));
        public double Line1X2 { get { return (double)GetValue(Line1X2Property); } set { SetValue(Line1X2Property, value); } }
        public static readonly DependencyProperty Line1Y2Property = DependencyProperty.Register("Line1Y2", typeof(double), typeof(AngleBorder));
        public double Line1Y2 { get { return (double)GetValue(Line1Y2Property); } set { SetValue(Line1Y2Property, value); } }

        public static readonly DependencyProperty Line2X1Property = DependencyProperty.Register("Line2X1", typeof(double), typeof(AngleBorder));
        public double Line2X1 { get { return (double)GetValue(Line2X1Property); } set { SetValue(Line2X1Property, value); } }
        public static readonly DependencyProperty Line2Y1Property = DependencyProperty.Register("Line2Y1", typeof(double), typeof(AngleBorder));
        public double Line2Y1 { get { return (double)GetValue(Line2Y1Property); } set { SetValue(Line2Y1Property, value); } }
        public static readonly DependencyProperty Line2X2Property = DependencyProperty.Register("Line2X2", typeof(double), typeof(AngleBorder));
        public double Line2X2 { get { return (double)GetValue(Line2X2Property); } set { SetValue(Line2X2Property, value); } }
        public static readonly DependencyProperty Line2Y2Property = DependencyProperty.Register("Line2Y2", typeof(double), typeof(AngleBorder));
        public double Line2Y2 { get { return (double)GetValue(Line2Y2Property); } set { SetValue(Line2Y2Property, value); } }

        public static readonly DependencyProperty Line3X1Property = DependencyProperty.Register("Line3X1", typeof(double), typeof(AngleBorder));
        public double Line3X1 { get { return (double)GetValue(Line3X1Property); } set { SetValue(Line3X1Property, value); } }
        public static readonly DependencyProperty Line3Y1Property = DependencyProperty.Register("Line3Y1", typeof(double), typeof(AngleBorder));
        public double Line3Y1 { get { return (double)GetValue(Line3Y1Property); } set { SetValue(Line3Y1Property, value); } }
        public static readonly DependencyProperty Line3X2Property = DependencyProperty.Register("Line3X2", typeof(double), typeof(AngleBorder));
        public double Line3X2 { get { return (double)GetValue(Line3X2Property); } set { SetValue(Line3X2Property, value); } }
        public static readonly DependencyProperty Line3Y2Property = DependencyProperty.Register("Line3Y2", typeof(double), typeof(AngleBorder));
        public double Line3Y2 { get { return (double)GetValue(Line3Y2Property); } set { SetValue(Line3Y2Property, value); } }

        public static readonly DependencyProperty Line4X1Property = DependencyProperty.Register("Line4X1", typeof(double), typeof(AngleBorder));
        public double Line4X1 { get { return (double)GetValue(Line4X1Property); } set { SetValue(Line4X1Property, value); } }
        public static readonly DependencyProperty Line4Y1Property = DependencyProperty.Register("Line4Y1", typeof(double), typeof(AngleBorder));
        public double Line4Y1 { get { return (double)GetValue(Line4Y1Property); } set { SetValue(Line4Y1Property, value); } }
        public static readonly DependencyProperty Line4X2Property = DependencyProperty.Register("Line4X2", typeof(double), typeof(AngleBorder));
        public double Line4X2 { get { return (double)GetValue(Line4X2Property); } set { SetValue(Line4X2Property, value); } }
        public static readonly DependencyProperty Line4Y2Property = DependencyProperty.Register("Line4Y2", typeof(double), typeof(AngleBorder));
        public double Line4Y2 { get { return (double)GetValue(Line4Y2Property); } set { SetValue(Line4Y2Property, value); } }

        public static readonly DependencyProperty Line5X1Property = DependencyProperty.Register("Line5X1", typeof(double), typeof(AngleBorder));
        public double Line5X1 { get { return (double)GetValue(Line5X1Property); } set { SetValue(Line5X1Property, value); } }
        public static readonly DependencyProperty Line5Y1Property = DependencyProperty.Register("Line5Y1", typeof(double), typeof(AngleBorder));
        public double Line5Y1 { get { return (double)GetValue(Line5Y1Property); } set { SetValue(Line5Y1Property, value); } }
        public static readonly DependencyProperty Line5X2Property = DependencyProperty.Register("Line5X2", typeof(double), typeof(AngleBorder));
        public double Line5X2 { get { return (double)GetValue(Line5X2Property); } set { SetValue(Line5X2Property, value); } }
        public static readonly DependencyProperty Line5Y2Property = DependencyProperty.Register("Line5Y2", typeof(double), typeof(AngleBorder));
        public double Line5Y2 { get { return (double)GetValue(Line5Y2Property); } set { SetValue(Line5Y2Property, value); } }

        public static readonly DependencyProperty Line6X1Property = DependencyProperty.Register("Line6X1", typeof(double), typeof(AngleBorder));
        public double Line6X1 { get { return (double)GetValue(Line6X1Property); } set { SetValue(Line6X1Property, value); } }
        public static readonly DependencyProperty Line6Y1Property = DependencyProperty.Register("Line6Y1", typeof(double), typeof(AngleBorder));
        public double Line6Y1 { get { return (double)GetValue(Line6Y1Property); } set { SetValue(Line6Y1Property, value); } }
        public static readonly DependencyProperty Line6X2Property = DependencyProperty.Register("Line6X2", typeof(double), typeof(AngleBorder));
        public double Line6X2 { get { return (double)GetValue(Line6X2Property); } set { SetValue(Line6X2Property, value); } }
        public static readonly DependencyProperty Line6Y2Property = DependencyProperty.Register("Line6Y2", typeof(double), typeof(AngleBorder));
        public double Line6Y2 { get { return (double)GetValue(Line6Y2Property); } set { SetValue(Line6Y2Property, value); } }

        public static readonly DependencyProperty Line7X1Property = DependencyProperty.Register("Line7X1", typeof(double), typeof(AngleBorder));
        public double Line7X1 { get { return (double)GetValue(Line7X1Property); } set { SetValue(Line7X1Property, value); } }
        public static readonly DependencyProperty Line7Y1Property = DependencyProperty.Register("Line7Y1", typeof(double), typeof(AngleBorder));
        public double Line7Y1 { get { return (double)GetValue(Line7Y1Property); } set { SetValue(Line7Y1Property, value); } }
        public static readonly DependencyProperty Line7X2Property = DependencyProperty.Register("Line7X2", typeof(double), typeof(AngleBorder));
        public double Line7X2 { get { return (double)GetValue(Line7X2Property); } set { SetValue(Line7X2Property, value); } }
        public static readonly DependencyProperty Line7Y2Property = DependencyProperty.Register("Line7Y2", typeof(double), typeof(AngleBorder));
        public double Line7Y2 { get { return (double)GetValue(Line7Y2Property); } set { SetValue(Line7Y2Property, value); } }

        public static readonly DependencyProperty Line8X1Property = DependencyProperty.Register("Line8X1", typeof(double), typeof(AngleBorder));
        public double Line8X1 { get { return (double)GetValue(Line8X1Property); } set { SetValue(Line8X1Property, value); } }
        public static readonly DependencyProperty Line8Y1Property = DependencyProperty.Register("Line8Y1", typeof(double), typeof(AngleBorder));
        public double Line8Y1 { get { return (double)GetValue(Line8Y1Property); } set { SetValue(Line8Y1Property, value); } }
        public static readonly DependencyProperty Line8X2Property = DependencyProperty.Register("Line8X2", typeof(double), typeof(AngleBorder));
        public double Line8X2 { get { return (double)GetValue(Line8X2Property); } set { SetValue(Line8X2Property, value); } }
        public static readonly DependencyProperty Line8Y2Property = DependencyProperty.Register("Line8Y2", typeof(double), typeof(AngleBorder));
        public double Line8Y2 { get { return (double)GetValue(Line8Y2Property); } set { SetValue(Line8Y2Property, value); } }
        #endregion

        #endregion

        public AngleBorder()
        {
            #region 自适应尺寸
            Line1X1 = 0;
            Line1Y1 = -5;
            Line1X2 = 0;
            Line1Y2 = 40;

            Line2X1 = -5;
            Line2Y1 = 0;
            Line2X2 = 40;
            Line2Y2 = 0;

            Line3X1 = this.Width - 40;
            Line3Y1 = 0;
            Line3X2 = this.Width + 5;
            Line3Y2 = 40;

            Line4X1 = this.Width;
            Line4Y1 = -5;
            Line4X2 = -this.Width;
            Line4Y2 = 40;

            Line5X1 = this.Width;
            Line5Y1 = this.Height - 40;
            Line5X2 = this.Width;
            Line5Y2 = this.Height + 5;

            Line6X1 = this.Width - 40;
            Line6Y1 = this.Height;
            Line6X2 = this.Width + 5;
            Line6Y2 = this.Height;

            Line7X1 = -5;
            Line7Y1 = this.Height; ;
            Line7X2 = 40;
            Line7Y2 = this.Height; ;

            Line8X1 = 0;
            Line8Y1 = 0;
            Line8X2 = this.Height - 40;
            Line8Y2 = this.Width + 5;
            #endregion
        }
    }
}
