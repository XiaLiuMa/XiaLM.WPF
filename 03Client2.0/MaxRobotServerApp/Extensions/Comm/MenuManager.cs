using MaxRobotServerApp.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Extensions.Comm
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuManager
    {
        #region 单例
        private static MenuManager instance;
        private readonly static object objLock = new object();
        public static MenuManager GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new MenuManager();
                    }
                }
            }
            return instance;
        }

        public MenuManager()
        {
            ChildMenus = new List<MenuInfo>();
            Menus = new List<MenuInfo>()
            {
                new MenuInfo(){ Tag="A0",Name="首页",Page="IndexPage"},
                new MenuInfo(){ Tag="A1",Name="机器人管理",Page="MenuHeaderPage",Img="jqr"},
                new MenuInfo(){ Tag="A1B1",Name="机器人管理",Page="RobotManagePage"},
                new MenuInfo(){ Tag="A1B2",Name="实时地图",Page="RobotMapPage"},
                new MenuInfo(){ Tag="A2",Name="语义库管理",Page="MenuHeaderPage",Img="yyk"},
                new MenuInfo(){ Tag="A2B1",Name="语义库管理",Page="SemanticLibraryPage"},
                new MenuInfo(){ Tag="A2B2",Name="语义管理",Page="SemanticPage"},
                new MenuInfo(){ Tag="A3",Name="报警管理",Page="MenuHeaderPage",Img="bj"},
                new MenuInfo(){ Tag="A3B1",Name="区域监控",Page="AlarmAreaMonitoringPage"},
                new MenuInfo(){ Tag="A3B2",Name="低温探测",Page="AlarmLowTemperaturePage"},
                new MenuInfo(){ Tag="A3B3",Name="人脸对比",Page="AlarmFaceContrastPage"},
                new MenuInfo(){ Tag="A3B4",Name="智能判图",Page="AlarmSmartFigurePage"},
                new MenuInfo(){ Tag="A3B5",Name="核辐射",Page="AlarmNuclearRadiationPage"},
                new MenuInfo(){ Tag="A3B6",Name="黑名单",Page="AlarmBlackListPage"},
                new MenuInfo(){ Tag="A4",Name="资源管理",Page="MenuHeaderPage",Img="zy"},
                new MenuInfo(){ Tag="A4B1",Name="宣讲资源",Page="ResourcesPreachPage"},
                new MenuInfo(){ Tag="A4B2",Name="App资源",Page="ResourcesAppPage"},
                new MenuInfo(){ Tag="A5",Name="地图管理",Page="MenuHeaderPage",Img="dt"},
                new MenuInfo(){ Tag="A5B1",Name="地图信息管理",Page="MapManagePage"},
                new MenuInfo(){ Tag="A6",Name="任务管理",Page="MenuHeaderPage",Img="rw"},
                new MenuInfo(){ Tag="A6B1",Name="任务信息管理",Page="TaskManagePage"},
                new MenuInfo(){ Tag="A7",Name="用户管理",Page="MenuHeaderPage",Img="yh"},
                new MenuInfo(){ Tag="A7B1",Name="用户管理",Page="UserManagePage"},
                new MenuInfo(){ Tag="A7B2",Name="角色管理",Page="UserPermissionsPage"},
                new MenuInfo(){ Tag="A8",Name="系统管理",Page="MenuHeaderPage",Img="xt"},
                new MenuInfo(){ Tag="A8B1",Name="本地配置",Page="SystemLocalConfigPage"},
                new MenuInfo(){ Tag="A8B2",Name="服务配置",Page="SystemServerConfigPage"}
            };
        }
        #endregion

        /// <summary>
        /// 主页标识
        /// </summary>
        public const string IndexTag = "A0";
        /// <summary>
        /// 默认菜单
        /// </summary>
        public List<MenuInfo> Menus { get; set; }
        /// <summary>
        /// 用于(MenuHeaderPage)页显示的子菜单
        /// </summary>
        public List<MenuInfo> ChildMenus { get; set; }
    }
}
