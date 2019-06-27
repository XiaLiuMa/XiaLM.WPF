using MaxRobotServerApp.Mods;
using MaxRobotServerApp.Views.ViewModels;
using MyCustomControl;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MaxRobotServerApp.Views.Flyouts
{
    /// <summary>
    /// SelectMapPositionFlyout.xaml 的交互逻辑
    /// </summary>
    public partial class SelectMapPositionFlyout : CBaseFlyout
    {
        public event Action<MapPositionMod> OnSelected;
        public SelectMapPositionFlyout()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public SelectMapPositionFlyout(string  mapid):this()
        {
            ViewInit(mapid);
        }
        /// <summary>
        /// 界面初始化
        /// </summary>
        private async void ViewInit(string mapid)
        {
            var obj = new SelectMapPositionViewModel();
            this.DataContext = obj;
            this.pager1.SelectionChangedEvent += Pager1_SelectionChangedEvent;
            obj.RefreshDataGrid(mapid);
        }

        private void Pager1_SelectionChangedEvent(int obj)
        {
            (this.DataContext as SelectMapPositionViewModel).PageSize = obj;
            (this.DataContext as SelectMapPositionViewModel).RefreshDataGrid();
        }

        //选择
        private void SelectedBtu_Click(object sender, RoutedEventArgs e)
        {
            var mod = ((Button)sender).DataContext as MapPositionMod;
            OnSelected?.Invoke(mod);
            this.Close();
        }
    }
}
