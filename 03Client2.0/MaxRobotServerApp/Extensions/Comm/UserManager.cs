using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods.Config;
using MaxRobotServerApp.Views.Flyouts;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Extensions.Comm
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    public class UserManager
    {
        #region 单例
        private static UserManager instance;
        private readonly static object objLock = new object();

        public static UserManager GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new UserManager();
                    }
                }
            }
            return instance;
        }

        #endregion

        private RobotClient _robotClient;
        /// <summary>
        /// sdk的RobotClient
        /// </summary>
        public RobotClient Rclient
        {
            get
            {
                if (_robotClient != null)
                {
                    return _robotClient;
                }
                else
                {
                    SystemConfig config = ConfigManager.GetInstance().LoadSystemConfig();
                    _robotClient = new RobotClient(config.ServerIp, config.ServerPort);
                    return _robotClient;
                } 
            }
            set
            {
                _robotClient = value;
            }
        }

        private UserInfo _User;
        /// <summary>
        /// 已登陆的用户
        /// </summary>
        public UserInfo User
        {
            get
            {
                if (_User == null)
                {
                    return new UserInfo();
                }
                else
                {
                    return _User;
                }
            }
            set
            {
                _User = value;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool RclientLogin(string user, string pwd)
        {
            var flag = Rclient.Login(user, pwd);
            if (flag)
            {
                UserManageDal dal = new UserManageDal();
                User = dal.Query(user);
            }
            else
            {
                _robotClient = null;
            }
            return flag;
        }

        public void RclientLoginOut()
        {
            Rclient.Logout();
            _robotClient = null;
            _User = null;
        }
    }
}
