using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SpecialEffectsControl.Ccontrols
{
    /// <summary>
    /// 下拉文本输入框
    /// </summary>
    public class DropdownTextBox : Control
    {
        static DropdownTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownTextBox), new FrameworkPropertyMetadata(typeof(DropdownTextBox)));
        }

        #region 属性

        public static readonly DependencyProperty IsDropdownOpenedProperty = DependencyProperty.Register("IsDropdownOpened", typeof(bool), typeof(DropdownTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(IsDropdownOpenedPropertyChanged)));

        /// <summary>
        /// 下拉框是否已打开
        /// </summary>
        public bool IsDropdownOpened
        {
            get { return (bool)GetValue(IsDropdownOpenedProperty); }
            set { SetValue(IsDropdownOpenedProperty, value); }
        }

        private static void IsDropdownOpenedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool b = (bool)e.NewValue;
            DropdownTextBox db = d as DropdownTextBox;
            db.OnDropdownStateChanged(b);
        }

        protected virtual void OnDropdownStateChanged(bool isopened)
        {
            // 引发路由事件
            RoutedEventArgs args = new RoutedEventArgs();
            if (isopened)
            {
                args.RoutedEvent = DropdownOpenedEvent;
            }
            else
            {
                args.RoutedEvent = DropdownClosedEvent;
            }
            args.Source = this;
            RaiseEvent(args);
        }

        public static readonly DependencyProperty DropItemsProperty = DependencyProperty.Register("DropItems", typeof(IEnumerable), typeof(DropdownTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(DropItemsPropertyChanged)));

        /// <summary>
        /// 获取或设置下拉框中显示的项
        /// </summary>
        public IEnumerable DropItems
        {
            get { return (IEnumerable)GetValue(DropItemsProperty); }
            set { SetValue(DropItemsProperty, value); }
        }

        private static void DropItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownTextBox dt = d as DropdownTextBox;
            if (dt != null)
            {
                IEnumerable oldval, newval;
                oldval = (IEnumerable)e.OldValue;
                newval = (IEnumerable)e.NewValue;
                dt.OnDropItemsChanged(oldval, newval);
            }
        }

        protected virtual void OnDropItemsChanged(IEnumerable oldItems, IEnumerable newItems)
        {
            if (oldItems != newItems)
            {
                this.UpdateUIItems(newItems);
            }
        }

        public static readonly DependencyProperty MinDropdownHeightProperty = DependencyProperty.Register("MinDropdownHeighte", typeof(double), typeof(DropdownTextBox));

        /// <summary>
        /// 下拉列表框的最小高度
        /// </summary>
        public double MinDropdownHeighte
        {
            get { return (double)GetValue(MinDropdownHeightProperty); }
            set { SetValue(MinDropdownHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxDropdownHeightProperty = DependencyProperty.Register("MaxDropdownHeight", typeof(double), typeof(DropdownTextBox));

        /// <summary>
        /// 下拉列表框的最大高度
        /// </summary>
        public double MaxDropdownHeight
        {
            get { return (double)GetValue(MaxDropdownHeightProperty); }
            set { SetValue(MaxDropdownHeightProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = TextBox.TextProperty.AddOwner(typeof(DropdownTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownTextBox db = d as DropdownTextBox;
            db.OnTextChanged();
        }

        /// <summary>
        /// 获取或设置控件中显示的文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region 字段
        /// <summary>
        /// 文本输入框
        /// </summary>
        private TextBox m_InputBox = null;
        /// <summary>
        /// 模板中文本框控件的名字
        /// </summary>
        const string PART_InputBox = "PART_InputBox";
        /// <summary>
        /// 显示项的面板
        /// </summary>
        const string PART_ItemPanel = "PART_ItemPanel";
        /// <summary>
        /// 显示项的面板
        /// </summary>
        private Panel m_itemsPanel = null;
        #endregion

        #region 内部方法
        /// <summary>
        /// 创建项容器
        /// </summary>
        /// <returns></returns>
        protected virtual UIElement CreateItemContainer()
        {
            DropdownItem ue = new DropdownItem();
            AddItemActivedHandler(ue, new RoutedEventHandler(Item_Actived));
            return ue;
        }

        private void Item_Actived(object sender, RoutedEventArgs e)
        {
            if (m_InputBox == null) return;
            DropdownItem item = e.Source as DropdownItem;
            if (item != null)
            {
                string str = item.NormalText;
                m_InputBox.Tag = true;
                m_InputBox.Text = str;
                m_InputBox.Tag = false;
                IsDropdownOpened = false;
            }
        }

        protected virtual void ClearAllItems()
        {
            if (m_itemsPanel != null)
            {
                m_itemsPanel.Children.Clear();
            }
        }

        /// <summary>
        /// 更新项列表UI
        /// </summary>
        private void UpdateUIItems(IEnumerable items)
        {
            if (m_itemsPanel != null)
            {
                ClearAllItems();
                if (items != null)
                {
                    foreach (object item in items)
                    {
                        string strContent = string.Empty;
                        if (item is string)
                            strContent = item as string;
                        else
                            strContent = item.ToString();
                        UIElement ui = CreateItemContainer();
                        ui.SetValue(DropdownItem.NormalTextProperty, strContent);
                        m_itemsPanel.Children.Add(ui);
                    }
                    m_itemsPanel.UpdateLayout();
                    //m_itemsPanel.InvalidateMeasure();
                    //m_itemsPanel.InvalidateArrange();
                }
            }
        }

        public override void OnApplyTemplate()
        {
            if (m_InputBox != null)
            {
                m_InputBox.TextChanged -= m_InputBox_TextChanged;
                m_InputBox = null;
            }
            m_InputBox = GetTemplateChild(PART_InputBox) as TextBox;
            if (m_InputBox != null)
                m_InputBox.Tag = false;
            m_InputBox.TextChanged += m_InputBox_TextChanged;

            if (m_itemsPanel != null)
            {
                m_itemsPanel = null;
            }
            m_itemsPanel = GetTemplateChild(PART_ItemPanel) as VirtualizingStackPanel;
            if (m_itemsPanel != null)
            {
                UpdateUIItems(DropItems);
            }

            base.OnApplyTemplate();
        }

        void m_InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool b = (bool)m_InputBox.Tag;
            if (b == false)
            {
                if (IsDropdownOpened == false)
                {
                    IsDropdownOpened = true;
                }
            }

            FilterText(m_InputBox.Text);

            // 引发本类的TextChanged事件
            //OnTextChanged(e);
        }

        /// <summary>
        /// 高亮显示字符
        /// </summary>
        /// <param name="txt"></param>
        private void FilterText(string txt)
        {
            if (m_itemsPanel != null)
            {
                foreach (UIElement ue in m_itemsPanel.Children)
                {
                    ue.SetValue(DropdownItem.FilterTextProperty, txt);
                }
            }
        }

        protected virtual void OnTextChanged()
        {
            RoutedEventArgs args = new RoutedEventArgs(TextChangedEvent, this);
            RaiseEvent(args);
        }

        #endregion

        #region 事件
        public static readonly RoutedEvent ItemActivedEvent = EventManager.RegisterRoutedEvent("ItemActived", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropdownTextBox));

        public static void AddItemActivedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement u = d as UIElement;
            if (u != null)
            {
                u.AddHandler(ItemActivedEvent, handler);
            }
        }

        public static void RemoveItemActivedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement u = d as UIElement;
            if (u != null)
            {
                u.RemoveHandler(ItemActivedEvent, handler);
            }
        }


        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropdownTextBox));

        /// <summary>
        /// 当文本框中的内容改变时发生
        /// </summary>
        public event RoutedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }

        public static readonly RoutedEvent DropdownOpenedEvent = EventManager.RegisterRoutedEvent("DropdownOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropdownTextBox));

        /// <summary>
        /// 当下拉框打下后发生
        /// </summary>
        public event RoutedEventHandler DropdownOpened
        {
            add { AddHandler(DropdownOpenedEvent, value); }
            remove { RemoveHandler(DropdownOpenedEvent, value); }
        }

        public static readonly RoutedEvent DropdownClosedEvent = EventManager.RegisterRoutedEvent("DropdownClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropdownTextBox));

        /// <summary>
        /// 当下拉框关闭后发生
        /// </summary>
        public event RoutedEventHandler DropdownClosed
        {
            add { AddHandler(DropdownClosedEvent, value); }
            remove { RemoveHandler(DropdownClosedEvent, value); }
        }

        #endregion
    }



    /// <summary>
    /// 下拉项
    /// </summary>
    public class DropdownItem : Control
    {
        static DropdownItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownItem), new FrameworkPropertyMetadata(typeof(DropdownItem)));
        }

        #region 字段
        public const string PART_Textpresenter = "PART_Textpresenter";
        private TextBlock m_Textpresenter = null;

        const string TAG_NORMALTEXT = "Normaltext";
        const string TAG_FILTERTEXT = "Filtertext";
        #endregion

        #region 属性
        public static readonly DependencyProperty NormalBrushProperty = DependencyProperty.Register("NormalBrush", typeof(Brush), typeof(DropdownItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(NormalBrushChanged)));

        private static void NormalBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownItem ditem = d as DropdownItem;
            ditem.OnNormalBrushChanged();
        }

        public Brush NormalBrush
        {
            get { return (Brush)GetValue(NormalBrushProperty); }
            set
            {
                SetValue(NormalBrushProperty, value);
            }
        }

        private void OnNormalBrushChanged()
        {
            if (m_Textpresenter != null)
            {
                foreach (var item in m_Textpresenter.Inlines)
                {
                    string tag = item.Tag as string;
                    if (tag.ToLower() == TAG_NORMALTEXT)
                    {
                        item.Foreground = NormalBrush;
                    }
                }
            }
        }
        public static readonly DependencyProperty FilterBrushProperty = DependencyProperty.Register("FilterBrush", typeof(Brush), typeof(DropdownItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(FilterBrushChanged)));

        private static void FilterBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownItem dropitem = d as DropdownItem;
            dropitem.OnFilterBrushChanged();
        }
        public Brush FilterBrush
        {
            get { return (Brush)GetValue(FilterBrushProperty); }
            set
            {
                SetValue(FilterBrushProperty, value);
            }

        }

        private void OnFilterBrushChanged()
        {
            if (m_Textpresenter != null)
            {
                foreach (var item in m_Textpresenter.Inlines)
                {
                    string tag = item.Tag as string;
                    if (tag == TAG_FILTERTEXT)
                    {
                        item.Foreground = FilterBrush;
                    }
                }
            }
        }
        public static readonly DependencyProperty NormalTextProperty = DependencyProperty.Register("NormalText", typeof(string), typeof(DropdownItem),
            new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(NormalTextChanged)));

        private static void NormalTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownItem ditem = d as DropdownItem;
            if (e.NewValue != e.OldValue)
            {
                string val = e.NewValue as string;
                ditem.OnNormalTextChanged(val);
            }
        }

        /// <summary>
        /// 项的文本
        /// </summary>
        public string NormalText
        {
            get { return (string)GetValue(NormalTextProperty); }
            set
            {
                SetValue(NormalTextProperty, value);
            }
        }

        private void OnNormalTextChanged(string text)
        {
            if (m_Textpresenter != null)
            {
                m_Textpresenter.Text = text;
            }
        }

        public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register("FilterText", typeof(string), typeof(DropdownItem), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(FilterTextChanged)));

        private static void FilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DropdownItem di = d as DropdownItem;
            if (e.NewValue != e.OldValue)
            {
                string str = e.NewValue as string;
                di.FilterTextCore(str);
            }
        }


        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }
        #endregion


        #region 方法
        /// <summary>
        /// 过滤字符颜色
        /// </summary>
        /// <param name="filter"></param>
        private void FilterTextCore(string filter)
        {
            if (m_Textpresenter == null) return;
            if (string.IsNullOrEmpty(NormalText)) return;
            if (string.IsNullOrWhiteSpace(filter)) return;

            m_Textpresenter.Inlines.Clear();
            char[] chars = NormalText.ToCharArray();
            foreach (char c in chars)
            {
                Run runTxt = new Run();
                runTxt.Text = c.ToString();
                if (filter.Contains(c)) //关键字
                {
                    runTxt.Foreground = FilterBrush;
                    runTxt.Tag = TAG_FILTERTEXT;
                }
                else     //正常显示的字符
                {
                    runTxt.Foreground = NormalBrush;
                    runTxt.Tag = TAG_NORMALTEXT;
                }
                m_Textpresenter.Inlines.Add(runTxt);
            }
        }

        public override void OnApplyTemplate()
        {
            m_Textpresenter = GetTemplateChild(PART_Textpresenter) as TextBlock;
            if (m_Textpresenter != null)
            {
                m_Textpresenter.Text = NormalText;
            }
            base.OnApplyTemplate();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            RoutedEventArgs arg = new RoutedEventArgs(DropdownTextBox.ItemActivedEvent, this);
            this.RaiseEvent(arg);
            e.Handled = true;
        }
        #endregion
    }
}
