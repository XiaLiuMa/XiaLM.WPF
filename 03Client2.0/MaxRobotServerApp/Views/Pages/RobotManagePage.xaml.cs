
using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Views.Flyouts;
using MaxRobotServerApp.Views.Infos;
using MaxRobotServerApp.Views.ViewModels;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
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

namespace MaxRobotServerApp.Views.Pages
{
    /// <summary>
    /// RobotManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class RobotManagePage : Page
    {
        /// <summary>
        /// 机器人列表
        /// </summary>
        private List<RobotInfo> lst = new List<RobotInfo>();

        public RobotManagePage()
        {
            InitializeComponent();
            RobotManagement management = new RobotManagement();
            management.Online += Management_Online;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                RobotMangerDal dal = new RobotMangerDal();
                lst = dal.GetRobotList();
                RefreshList(lst);
            });
        }

        /// <summary>
        /// 机器人状态变化事件
        /// </summary>
        /// <param name="onlineInfo"></param>
        private void Management_Online(RobotOnlineInfo onlineInfo)
        {
            try
            {
                var robot = lst.Where(p => p.Tag.Equals(onlineInfo.Tag)).FirstOrDefault();
                if (robot != null)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (var itm in this.robotgrid.Children)
                        {
                            var model = (itm as URobotInfo).DataContext as URobotInfoViewModel;
                            if (model.Tag.Equals(onlineInfo.Tag))
                            {
                                model.RefreshRobotInfo(onlineInfo);
                            }
                        }
                    }));
                }
                else //新增的上线机器人
                {
                    RobotMangerDal dal = new RobotMangerDal();
                    RobotInfo obj = dal.GetRobotInfo(onlineInfo.Tag);
                    lst.Add(obj);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        RefreshList(lst);
                    }));
                    Management_Online(onlineInfo);  //添加完列表后补充上状态
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshList(List<RobotInfo> lst)
        {
            this.Dispatcher?.Invoke(new Action(() =>
            {
                this.robotgrid.Children.Clear();
                if (lst == null || lst.Count <= 0) return;

                int cnum = 4;   //列数
                int rnum = (lst.Count % cnum == 0) ? (lst.Count / cnum) : ((lst.Count / cnum) + 1);//行数
                for (int i = 0; i < rnum; i++)
                {
                    this.robotgrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(220) }); //增加行
                }

                for (int i = 0; i < lst.Count; i++)
                {
                    int row = i / cnum;    //行号-->除数
                    int colum = (i < cnum) ? i : (i % cnum);  //列号-->余数
                    URobotInfo robotInfo = new URobotInfo(new URobotInfoViewModel(lst[i]));
                    this.robotgrid.Children.Add(robotInfo); //添加到Grid控件
                    robotInfo.SetValue(Grid.RowProperty, row); //设置控件所在Grid的行
                    robotInfo.SetValue(Grid.ColumnProperty, colum); //设置控件所在Grid的列
                    robotInfo.VerticalAlignment = VerticalAlignment.Center;
                    robotInfo.HorizontalAlignment = HorizontalAlignment.Center;
                    robotInfo.RemoveRobotByTag += RobotInfo_RemoveRobotByTag;
                }
            }));
        }

        /// <summary>
        /// 移除机器人
        /// </summary>
        /// <param name="obj"></param>
        private void RobotInfo_RemoveRobotByTag(string obj)
        {
            RobotMangerDal dal = new RobotMangerDal();
            bool flag = dal.DeleteRobotInfo(obj);   //删除数据库的机器人信息
            if (flag)
            {
                lst.Remove(lst.Where(p => p.Tag.Equals(obj)).FirstOrDefault());
                RefreshList(lst);
            }
            else
            {
                new Warning("移除机器人失败!").ShowDialog();
            }
        }
    }
}
