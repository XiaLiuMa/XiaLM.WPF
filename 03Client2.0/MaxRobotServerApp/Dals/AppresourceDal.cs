using MaxRobotServerApp.Mods;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class AppresourceDal
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public OutPage<AppInfo> GetAppList(int currentPage, int pageSize)
        {
            AppManagement management = new AppManagement();
            OutPage<AppInfo> outPage = management.GetAppList(new InPage()
            {
                Current = currentPage,
                Row = pageSize
            });
            return outPage;
        }

        /// <summary>
        /// 新增App
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool Add(AppParms parms)
        {
            AppManagement management = new AppManagement();
            return management.Add(parms);
        }

        /// <summary>
        /// 删除App
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(int[] ids)
        {
            AppManagement management = new AppManagement();
            return management.Delete(ids);
        }

        /// <summary>
        /// 修改App
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool Updata(AppParms parms)
        {
            AppManagement management = new AppManagement();
            return management.Updata(parms);
        }

    }
}
