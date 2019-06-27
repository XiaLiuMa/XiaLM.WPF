using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class UserManageDal
    {
        /// <summary>
        /// 根据用户名，名查询用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UserInfo Query(string name)
        {
            UserManagmement management = new UserManagmement();
            return management.Query(name);
        }

        /// <summary>
        /// 得到用户列表
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public OutPage<UserInfo> GetList(int currentPage, int pageSize)
        {
            UserManagmement management = new UserManagmement();
            OutPage<UserInfo> outPage = management.GetList(new InPage()
            {
                Current = currentPage,
                Row = pageSize
            });
            return outPage;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool Add(UserParms parms)
        {
            UserManagmement management = new UserManagmement();
            return management.Add(parms);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(int[] ids)
        {
            UserManagmement management = new UserManagmement();
            return management.Delete(ids);
        }


        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool Updata(UserParms parms)
        {
            UserManagmement management = new UserManagmement();
            return management.Updata(parms);
        }


        /// <summary>
        /// 验证密码是否正确.用于修改密码验证老密码是否正确
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool Verify(UserParms parms)
        {
            UserManagmement management = new UserManagmement();
            return management.Verify(parms);
        }
    }
}
