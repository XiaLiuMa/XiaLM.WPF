using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyCustomControl
{
    /// <summary>
    /// AngleBorder.xaml 的交互逻辑
    /// </summary>
    public class AngleBorder : Control
    {
        static AngleBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AngleBorder), new FrameworkPropertyMetadata(typeof(AngleBorder)));
        }

        #region 依赖属性
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(AngleBorder), null);
        /// <summary>
        /// 图标
        /// </summary>
        public ImageSource Icon { get { return (ImageSource)GetValue(IconProperty); } set { SetValue(IconProperty, value); } }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(AngleBorder), new PropertyMetadata("Title1"));
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get { return (string)GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }

        public static readonly DependencyProperty Line1Point1Property = DependencyProperty.Register("Line1Point1", typeof(Point), typeof(AngleBorder));
        /// <summary>
        /// Line1
        /// </summary>
        public Point Line1Point1 { get { return (Point)GetValue(Line1Point1Property); } set { SetValue(Line1Point1Property, value); } }

        public static readonly DependencyProperty ContentGridProperty = DependencyProperty.Register("ContentGrid", typeof(Grid), typeof(AngleBorder));
        /// <summary>
        /// 正文
        /// </summary>
        public Grid ContentGrid { get { return (Grid)GetValue(ContentGridProperty); } set { SetValue(ContentGridProperty, value); } }
        #endregion

        //public AngleBorder()
        //{
        //    this.Style = (Style)Application.Current.Resources["AngleBorderStyle"];   //加载样式

        //    #region 自适应尺寸
        //    //Line1X1 = 0;
        //    //Line1Y1 = -5;
        //    //Line1X2 = 0;
        //    //Line1Y2 = 40;

        //    //Line2X1 = -5;
        //    //Line2Y1 = 0;
        //    //Line2X2 = 40;
        //    //Line2Y2 = 0;

        //    //Line3X1 = this.Width - 40;
        //    //Line3Y1 = 0;
        //    //Line3X2 = this.Width + 5;
        //    //Line3Y2 = 40;

        //    //Line4X1 = this.Width;
        //    //Line4Y1 = -5;
        //    //Line4X2 = -this.Width;
        //    //Line4Y2 = 40;

        //    //Line5X1 = this.Width;
        //    //Line5Y1 = this.Height - 40;
        //    //Line5X2 = this.Width;
        //    //Line5Y2 = this.Height + 5;

        //    //Line6X1 = this.Width - 40;
        //    //Line6Y1 = this.Height;
        //    //Line6X2 = this.Width + 5;
        //    //Line6Y2 = this.Height;

        //    //Line7X1 = -5;
        //    //Line7Y1 = this.Height; ;
        //    //Line7X2 = 40;
        //    //Line7Y2 = this.Height; ;

        //    //Line8X1 = 0;
        //    //Line8Y1 = 0;
        //    //Line8X2 = this.Height - 40;
        //    //Line8Y2 = this.Width + 5;
        //    #endregion
        //}
    }
}
