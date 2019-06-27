using MyCustomControl;

using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// TaskTypeSelectFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class TaskTypeSelectFlyout : CBaseFlyout
    {

        public TaskTypeSelectFlyout()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //var data = new SelectData();
            this.DataContext = new SelectData();
        }
        private Action Refresh;
        public TaskTypeSelectFlyout(Action refresh) :this(){
            Refresh = refresh;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var obj = this.cbSelect.SelectedItem;
            var ss = obj as Select;
            switch (ss.TaskType)
            {
                case TaskType.cruise:
                    {
                        var flyout = new TaskCruiseFlyout(Refresh);
                        this.Close();
                        flyout.ShowDialog();
                   
                        break;
                    }
                case TaskType.line:
                    {
                        var flyout = new TaskLineFlyout(Refresh);
                        this.Close();
                        flyout.ShowDialog();
                        break;
                    }
                case TaskType.point:
                    {
                        var flyout = new TaskPositionFlyout(Refresh);
                        this.Close();
                        flyout.ShowDialog();
                        break;
                    }
                default:
                    break;
            }
        }
    }
    public class SelectData
    {
        public List<Select> Selects { get; set; }
        public SelectData()
        {
            Selects = new List<Select>();
            Selects.Add(new Select() { TaskType = TaskType.cruise, Name = "巡航任务" });
            Selects.Add(new Select() { TaskType = TaskType.line, Name = "连续线任务" });
            Selects.Add(new Select() { TaskType = TaskType.point, Name = "连续点任务" });
        }

    }
    public class Select
    {
        public TaskType TaskType { get; set; }
        public string Name { get; set; }
    }
}
