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
    /// 分页控件
    /// </summary>
    public partial class Pager : UserControl
    {
        public event Action<int> SelectionChangedEvent = (p) => { };
        //public static RoutedEvent SelectionChangedEvent;
        public static RoutedEvent FirstPageEvent;
        public static RoutedEvent PreviousPageEvent;
        public static RoutedEvent NextPageEvent;
        public static RoutedEvent LastPageEvent;

        public static readonly DependencyProperty CurrentPageProperty;
        public static readonly DependencyProperty TotalPageProperty;

        /// <summary>
        /// 当前页数
        /// </summary>
        public string CurrentPage
        {
            get { return (string)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public string TotalPage
        {
            get { return (string)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }

        public Pager()
        {
            InitializeComponent();
        }

        static Pager()
        {
            //SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));

            FirstPageEvent = EventManager.RegisterRoutedEvent("FirstPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            PreviousPageEvent = EventManager.RegisterRoutedEvent("PreviousPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            NextPageEvent = EventManager.RegisterRoutedEvent("NextPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            LastPageEvent = EventManager.RegisterRoutedEvent("LastPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));

            CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnCurrentPageChanged)));
            TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTotalPageChanged)));
        }

        //public event RoutedEventHandler SelectionChanged
        //{
        //    add { AddHandler(SelectionChangedEvent, value); }
        //    remove { RemoveHandler(SelectionChangedEvent, value); }
        //}

        public event RoutedEventHandler FirstPage
        {
            add { AddHandler(FirstPageEvent, value); }
            remove { RemoveHandler(FirstPageEvent, value); }
        }

        public event RoutedEventHandler PreviousPage
        {
            add { AddHandler(PreviousPageEvent, value); }
            remove { RemoveHandler(PreviousPageEvent, value); }
        }

        public event RoutedEventHandler NextPage
        {
            add { AddHandler(NextPageEvent, value); }
            remove { RemoveHandler(NextPageEvent, value); }
        }

        public event RoutedEventHandler LastPage
        {
            add { AddHandler(LastPageEvent, value); }
            remove { RemoveHandler(LastPageEvent, value); }
        }

        public static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if (p != null)
            {
                Run rTotal = (Run)p.FindName("rTotal");

                rTotal.Text = (string)e.NewValue;
            }
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if (p != null)
            {
                Run rCurrrent = (Run)p.FindName("rCurrent");

                rCurrrent.Text = (string)e.NewValue;
            }
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FirstPageEvent, this));
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PreviousPageEvent, this));
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NextPageEvent, this));
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LastPageEvent, this));
        }

        /// <summary>
        /// 每页行数变化时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowsComBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = this.rowsComBox.SelectedIndex;
            if (index < 0) return;
            int pageSize = (index * index + 1) * 5;
            SelectionChangedEvent(pageSize);
        }
    }
}
