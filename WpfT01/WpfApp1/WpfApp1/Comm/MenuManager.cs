using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Mods;

namespace WpfApp1.Comm
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
                new MenuInfo(){ Tag="A1",Name="首页",Page="IndexPage"},
                new MenuInfo(){ Tag="A2",Name="用户管理",Page="MenuHeaderPage"},
                new MenuInfo(){ Tag="A2B1",Name="会员管理",Page="UserMemberPage"},
                new MenuInfo(){ Tag="A2B2",Name="角色管理",Page="UserRolePage"},
                new MenuInfo(){ Tag="A3",Name="报表管理",Page="MenuHeaderPage"},
                new MenuInfo(){ Tag="A3B1",Name="LiveCharts",Page="LiveChartsPage"},
                new MenuInfo(){ Tag="A3B2",Name="ModernCharts",Page="ModernChartsPage"},
                new MenuInfo(){ Tag="A4",Name="热力区域",Page="MenuHeaderPage"},
                new MenuInfo(){ Tag="A4B1",Name="热力区域1",Page="HeatingArea1Page"},
                new MenuInfo(){ Tag="A4B2",Name="热力区域2",Page="HeatingArea2Page"}
            };
        }
        #endregion

        /// <summary>
        /// 主页标识
        /// </summary>
        public const string IndexTag = "A1";
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
