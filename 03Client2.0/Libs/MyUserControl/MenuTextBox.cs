using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyUserControl
{
    public class MenuTextBox : TextBox
    {
        public MenuTextBox()
        {
            this.IsReadOnly = true; //只读
            this.MouseEnter += MenuTextBox_MouseEnter;
            this.MouseMove += MenuTextBox_MouseMove;
            this.PreviewMouseDown += MenuTextBox_PreviewMouseDown;
        }

        private void MenuTextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender != null)
            {
                (sender as TextBox).SelectAll();
                (sender as TextBox).Cursor = Cursors.Hand;
            }
        }

        private void MenuTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender != null)
            {
                (sender as TextBox).Cursor = null;
            }
        }

        /// <summary>
        /// 鼠标点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //TextBox tb = sender as TextBox;
            //if (tb != null)
            //{
            //    tb.Focus();
            //    e.Handled = true;
            //}
        }
    }
}
