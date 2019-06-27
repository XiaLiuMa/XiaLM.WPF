using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk.Model;
using MyCustomControl;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// SelectMapLineFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class SelectMapLineFlyout : CBaseFlyout
    {
        public event Action<MapLineMod> OnSelected;
        public SelectMapLineFlyout()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public SelectMapLineFlyout(string mapid, LineType lineType) : this()
        {
            ViewInit(mapid, lineType);
        }
        /// <summary>
        /// 界面初始化
        /// </summary>
        private async void ViewInit(string mapid, LineType lineType)
        {
            var obj = new SelectMapLineViewModel();
            this.DataContext = obj;
            this.pager1.SelectionChangedEvent += Pager1_SelectionChangedEvent;
            obj.RefreshDataGrid(mapid, lineType);
        }

        private void Pager1_SelectionChangedEvent(int obj)
        {
            (this.DataContext as SelectMapLineViewModel).PageSize = obj;
            (this.DataContext as SelectMapLineViewModel).RefreshDataGrid();
        }

        //选择
        private void SelectedBtu_Click(object sender, RoutedEventArgs e)
        {
            var mod = ((Button)sender).DataContext as MapLineMod;
            OnSelected?.Invoke(mod);
            this.Close();
        }

    }
}
