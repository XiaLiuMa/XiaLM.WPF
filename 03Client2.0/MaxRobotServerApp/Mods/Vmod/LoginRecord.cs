using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Mods.Vmod
{
    /// <summary>
    /// 用户登陆记录
    /// </summary>
    public class LoginRecord
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string uname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passwrd { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public string logintime { get; set; }
    }
}
