using MaxRobotServerApp.Dals;
using Maxvision.Robot.Sdk.Model;
using MyCustomControl;
using System;
using System.Windows;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// SemanticlibrarymanagerFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class SemanticlibrarymanagerFlyout : CBaseFlyout
    {
        /// <summary>
        /// 正在编辑的对象id
        /// </summary>
        private int Id { get; set; }
        /// <summary>
        /// 是否是编辑模式，否则是添加模式
        /// </summary>
        private bool IsEdit { get; set; }

        /// <summary>
        /// 提交成功事件
        /// </summary>
        public event Action ConfirmSuccess = () => { };

        public SemanticlibrarymanagerFlyout(SemanticGenreInfo obj)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ViewInit(obj);
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void ViewInit(SemanticGenreInfo obj)
        {
            if (obj != null)
            {
                Id = obj.Id;
                IsEdit = true;
                this.name.IsReadOnly = true;
                this.name.Text = obj.Name;
                this.desc.Text = obj.Desc;
                this.isenable.IsChecked = obj.Enable;
            }
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.name.Text.Trim())) return;  //未填用户名
            try
            {
                SemanticmanagerDal dal = new SemanticmanagerDal();
                SemanticGenreInfo parms = new SemanticGenreInfo()
                {
                    Name = this.name.Text.Trim(),
                    Desc = this.desc.Text.Trim(),
                    Enable = (bool)this.isenable.IsChecked
                };

                if (IsEdit)
                {
                    parms.Id = Id;
                    bool flag = dal.UpdataSemanticGenre(parms);
                    if (flag)
                    {
                        ConfirmSuccess();
                    }
                    else
                    {
                        new Warning("编辑语义库失败!").ShowDialog();
                    }
                }
                else
                {
                    bool flag = dal.AddSemanticGenre(parms);
                    if (flag)
                    {
                        ConfirmSuccess();
                    }
                    else
                    {
                        new Warning("添加语义库失败!").ShowDialog();
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
