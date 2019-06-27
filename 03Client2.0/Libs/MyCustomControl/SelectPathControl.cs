using System.Windows;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using DialogResult = System.Windows.Forms.DialogResult;
using Button = System.Windows.Controls.Button;
using Control = System.Windows.Controls.Control;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Windows.Media;
using System;

namespace MyCustomControl
{
    [TemplatePart(Name = "SelectBtn", Type = typeof(Button))]
    public class SelectPathControl : Control
    {
        public event Action<string> OnSelected;
        #region 属性

        public static readonly DependencyProperty PathProperty =
        DependencyProperty.Register("Path", typeof(string), typeof(SelectPathControl), new PropertyMetadata(string.Empty));
        /// <summary> 选择的路径
        /// </summary>
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }


        public static readonly DependencyProperty FilterProperty =
        DependencyProperty.Register("Filter", typeof(string), typeof(SelectPathControl), new PropertyMetadata("All|*.*"));
        /// <summary> 文件格式过滤器。
        /// </summary>
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty SelectModeProperty =
            DependencyProperty.Register("SelectMode", typeof(SelectModeType), typeof(SelectPathControl), new PropertyMetadata(SelectModeType.SelectFile));
        /// <summary> 选择格式
        /// </summary>
        public SelectModeType SelectMode
        {
            get { return (SelectModeType)GetValue(SelectModeProperty); }
            set { SetValue(SelectModeProperty, value); }
        }

        #endregion

        static SelectPathControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectPathControl), new FrameworkPropertyMetadata(typeof(SelectPathControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var btn = GetTemplateChild("SelectBtn") as Button;
            if (btn != null)
            {
                btn.Click += (s, e) =>
                {
                    switch (SelectMode)
                    {
                        case SelectModeType.SelectFile: OpenSelectFileDialog(); break;
                        case SelectModeType.SelectFolder: OpenSelectFolderDialog(); break;
                        case SelectModeType.SaveFile: OpenSaveFileDialog(); break;
                    }
                };
            }
        }

        #region 按钮相应

        /// <summary> 设置保存的文件名称
        /// </summary>
        private void OpenSaveFileDialog()
        {
            var dlg = new SaveFileDialog { Filter = Filter, FileName = Path };
            var res = dlg.ShowDialog();
            if (res != true) return;
            Path = dlg.FileName;
            OnSelected?.Invoke(Path);
        }

        /// <summary> 选择文件
        /// </summary>
        private void OpenSelectFileDialog()
        {
            var dlg = new OpenFileDialog { Filter = Filter, FileName = Path };
            var res = dlg.ShowDialog();
            if (res != true) return;
            Path = dlg.FileName;
            OnSelected?.Invoke(Path);
        }

        /// <summary> 选择文件夹
        /// </summary>
        private void OpenSelectFolderDialog()
        {
            var dlg = new FolderBrowserDialog { SelectedPath = Path };
            var res = dlg.ShowDialog() == DialogResult.OK;
            if (!res) return;
            Path = dlg.SelectedPath;
            OnSelected?.Invoke(Path);
        }

        #endregion
    }
    public enum SelectModeType
    {
        /// <summary> 选择文件
        /// </summary>
        SelectFile,
        /// <summary> 选择文件夹
        /// </summary>
        SelectFolder,
        /// <summary> 保存文件
        /// </summary>
        SaveFile
    }
}
