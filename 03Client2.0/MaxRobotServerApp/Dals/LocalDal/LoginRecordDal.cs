using MaxRobotServerApp.Dals.Sql;
using MaxRobotServerApp.Extensions.Utils;
using MaxRobotServerApp.Mods.Vmod;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MaxRobotServerApp.Dals.LocalDal
{
    /// <summary>
    /// 登陆记录Dal
    /// </summary>
    public class LoginRecordDal
    {
        private ISqlOperate sqlite;

        public LoginRecordDal()
        {
            sqlite = new SqliteSqlOperate();
        }

        /// <summary>
        /// 模糊查询登陆记录
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public List<LoginRecord> FuzzyQuery(string txt = "")
        {
            try
            {
                string sql = $"select * from LoginRecord where uname like '%{txt}%' order by logintime";
                DataSet ds = new DataSet();
                sqlite.RunSQL(sql, ref ds);
                DataSetUtil<LoginRecord> util = new DataSetUtil<LoginRecord>();
                List<LoginRecord> lst = util.DataSetToClassList(ds);
                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public LoginRecord GetInfo(string name)
        {
            try
            {
                string sql = $"select * from LoginRecord where uname like '{name}'";
                DataSet ds = new DataSet();
                sqlite.RunSQL(sql, ref ds);
                DataSetUtil<LoginRecord> util = new DataSetUtil<LoginRecord>();
                return util.DataSetToClassList(ds).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 新增登陆记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Add(LoginRecord obj)
        {
            try
            {
                if (GetInfo(obj.uname) == null) //新增
                {
                    string sql = $"insert into LoginRecord VALUES ('{obj.uname}','{obj.passwrd}','{obj.logintime}')";
                    int i = sqlite.RunSQL(sql);
                    bool flag = i <= -1 ? false : true;
                    return flag;
                }
                else //修改
                {
                    return Update(obj);
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 修改登陆记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Update(LoginRecord obj)
        {
            try
            {
                string sql = $"update LoginRecord set passwrd='{obj.passwrd}',logintime='{obj.logintime}' where uname like '{obj.uname}'";
                int i = sqlite.RunSQL(sql);
                bool flag = i <= -1 ? false : true;
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 删除登陆记录
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public bool Delete(string[] names)
        {
            try
            {
                string where = "";
                for (int j = 0; j < names.Length; j++)
                {
                    if (j != (names.Length - 1))
                    {
                        where += $"'{names[j]}',";
                    }
                    else
                    {
                        where += $"'{names[j]}'";
                    }
                }

                string sql = $"delete from LoginRecord where uname id in ({where})";
                int i = sqlite.RunSQL(sql);
                bool flag = i <= -1 ? false : true;
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 根据时间删除登陆记录
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public bool DeleteByTime(string time)
        {
            try
            {
                string sql = $"delete from LoginRecord where logintime=>'{time}'";
                int i = sqlite.RunSQL(sql);
                bool flag = i <= -1 ? false : true;
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
