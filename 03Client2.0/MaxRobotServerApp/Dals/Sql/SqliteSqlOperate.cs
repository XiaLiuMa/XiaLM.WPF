using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals.Sql
{
    /// <summary>
    /// 提供Sqlite的数据库操作接口实现
    /// </summary>
    public class SqliteSqlOperate : ISqlOperate
    {
        /// <summary>
        /// 默认连接字符串
        /// </summary>
        private string dbConStr = $@"Data Source={AppContext.BaseDirectory}\db\MRobotServer.db";
        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        public string DbConStr
        {
            get { return this.dbConStr; }
            set { dbConStr = value; }
        }

        private static object lckObj = new object();
        public int RunSQL(string cmdText)
        {
            int re = -1;
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new SQLiteConnection(dbConStr))
                    {
                        using (var cmd = new SQLiteCommand(cmdText, oleDb))
                        {
                            oleDb.Open();
                            re = cmd.ExecuteNonQuery();
                            return re;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return -1;
                }
            }
        }

        public void RunSQL(string cmdText, ref DataSet ds)
        {
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new SQLiteConnection(dbConStr))
                    {
                        using (var da = new SQLiteDataAdapter(cmdText, oleDb))
                        {
                            oleDb.Open();
                            da.Fill(ds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
