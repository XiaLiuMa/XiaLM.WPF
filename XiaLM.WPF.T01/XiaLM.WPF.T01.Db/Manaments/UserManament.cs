using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaLM.WPF.T01.Db.Models;
using XiaLM.WPF.T01.Db.UiModels;

namespace XiaLM.WPF.T01.Db.Manaments
{
    public class UserManament
    {
        private static UserManament instance;
        private readonly static object objLock = new object();
        public static UserManament GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new UserManament();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 免登陆(自动登陆)
        /// </summary>
        /// <returns></returns>
        public bool AutoLogin()
        {
            using (BaseDBContext dbContext = new BaseDBContext())
            {
                try
                {
                    var obj = dbContext.Users.Where(p1 => p1.IsAutoLogin).OrderByDescending(p1 => p1.LoginTime).FirstOrDefault();   //获取最后登陆的用户
                    if (obj != null)
                    {
                        TimeSpan tSpan = DateTime.Now - DateTime.Parse(obj.LoginTime);
                        if (tSpan.TotalMilliseconds < 1000 * 60 * 10)   //免登陆保持时间为10分钟
                        {
                            UserLoginData userLoginData = new UserLoginData()
                            {
                                Name = obj.Name,
                                Pwd = obj.Pwd
                            };
                            return UserLogin(userLoginData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //LogSettings.Error(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UserLogin(UserLoginData user)
        {
            using (BaseDBContext dbContext = new BaseDBContext())
            {
                try
                {
                    var obj = dbContext.Users.Where(d => d.Name.Equals(user.Name) && d.Pwd.Equals(user.Pwd)).FirstOrDefault();
                    if (obj != null)
                    {
                        obj.LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); //更新登陆时间
                        dbContext.SaveChanges();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //LogSettings.Error(ex);
                }
            }
            return false;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UserSign(UseSignData user)
        {
            using (BaseDBContext dbContext = new BaseDBContext())
            {
                try
                {
                    var obj = dbContext.Users.Where(d => d.Name.Equals(user.Name)).FirstOrDefault();  //用户名不能重复
                    if (obj != null) return false;

                    dbContext.Users.Add(new TB_User()
                    {
                        Name = user.Name,
                        Pwd = user.Name,
                        SignTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    });
                    var n = dbContext.SaveChanges();
                    if (n < 0) return false;
                    return true;
                }
                catch (Exception ex)
                {
                    //LogSettings.Error(ex);
                }
            }

            return false;
        }

        public object SelectUsers(string selectStr)
        {
            using (BaseDBContext dbContext = new BaseDBContext())
            {
                try
                {
                    List<TB_User> list = new List<TB_User>();
                    if (string.IsNullOrEmpty(selectStr))    //为空的话查询全部
                    {
                        list = dbContext.Users.ToList();
                    }
                    else
                    {
                        string[] typeArray = selectStr.Split(',');   //解析用逗号隔开的类型列表
                        for (int i = 0; i < typeArray.Length; i++)
                        {
                            string tempType = typeArray[i];
                            //list.AddRange(dbContext.Users.Where(r => r.Type.Equals(tempType)).ToList());
                        }
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    //LogSettings.Error(ex);
                }
            }

            return null;
        }
    }
}
