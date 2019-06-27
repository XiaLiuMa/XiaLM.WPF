using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SpecialEffectsControl.Ccontrols
{
    /// <summary>
    /// WPF弹窗口基类
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    public class FadingWindow : Window
    {
        #region 控件特性
        /// <summary>
        /// 淡入淡出方向
        /// </summary>
        public static readonly DependencyProperty FadingDirectionProperty = DependencyProperty.Register("FadingDirection", typeof(FadingDirection), typeof(FadingWindow));
        public FadingDirection FadingDirection
        {
            get { return (FadingDirection)GetValue(FadingDirectionProperty); }
            set { SetValue(FadingDirectionProperty, value); }
        } 
        #endregion

        public FadingWindow()
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
            InitAnimation();

            ControlTemplate TeryWindowTemplate = (ControlTemplate)Application.Current.Resources["TeryWindowControlTemplate"];

            TextBlock winTitle = (TextBlock)TeryWindowTemplate.FindName("winTitle", this);
            winTitle.Text = title;

            Image closeBtn = (Image)TeryWindowTemplate.FindName("btnClose", this);
            closeBtn.MouseDown += CloseBtn_MouseDown;

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

        /// <summary>
        /// 开始淡入动画
        /// </summary>
        private void InitAnimation()
        {
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    DoubleAnimationUsingKeyFrames dak = new DoubleAnimationUsingKeyFrames();    //关键帧定义
                    switch (FadingDirection)
                    {
                        case FadingDirection.LtoR:  //左到右                   
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                            this.BeginAnimation(Window.WidthProperty, dak);
                            break;
                        case FadingDirection.RtoL:  //右到左
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 1), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                            this.BeginAnimation(Window.WidthProperty, dak);
                            break;
                        case FadingDirection.TtoB:  //上到下
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                            this.BeginAnimation(Window.HeightProperty, dak);
                            break;
                        case FadingDirection.BtoT:  //下到上
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 1), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                            dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                            this.BeginAnimation(Window.HeightProperty, dak);
                            break;
                    }
                }));
            });
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimationUsingKeyFrames dak = new DoubleAnimationUsingKeyFrames();    //关键帧定义
            switch (FadingDirection)
            {
                case FadingDirection.LtoR:  //左到右 
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 1), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                    break;
                case FadingDirection.RtoL:  //右到左
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Width / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                    this.BeginAnimation(Window.WidthProperty, dak);
                    break;
                case FadingDirection.TtoB:  //上到下
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 1), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                    this.BeginAnimation(Window.HeightProperty, dak);
                    break;
                case FadingDirection.BtoT:  //下到上
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 2), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 3), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 4), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8))));
                    dak.KeyFrames.Add(new LinearDoubleKeyFrame((int)(this.Height / 5 * 5), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0))));
                    this.BeginAnimation(Window.HeightProperty, dak);
                    break;
            }

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Close();
                }));
            });
        }

        private void InitializeStyle()
        {
            this.Style = (Style)Application.Current.Resources["TeryWindowStyle"];
        }
    }

    /// <summary>
    /// 淡入淡出方向
    /// </summary>
    public enum FadingDirection
    {
        LtoR = 0,
        RtoL = 1,
        TtoB = 2,
        BtoT = 3,
    }
}
