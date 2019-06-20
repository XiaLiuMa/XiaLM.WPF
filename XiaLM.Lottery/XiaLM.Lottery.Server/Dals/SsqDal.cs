using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XiaLM.Lottery.Server.Entities;
using XiaLM.Lottery.Server.Extend.Sql;
using XiaLM.Util;

namespace XiaLM.Lottery.Server.Dals
{
    public class SsqDal
    {
        private ISqlOperate sqlite;

        public SsqDal()
        {
            string dbPath = $@"Data Source={AppContext.BaseDirectory}db\Lottery.db";
            sqlite = new SqliteSqlOperate(dbPath);
        }

        /// <summary>
        /// 刷新数据(没有就新增，有就修改)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool RefreshData(SsqEntity obj)
        {
            try
            {
                if (GetInfo(obj.expect) == null) //新增
                {
                    string sql = $"insert into SSQ VALUES ({obj.expect},{obj.red1},{obj.red2},{obj.red3},{obj.red4},{obj.red5},{obj.red6},{obj.blue1},{obj.time})";
                    int i = sqlite.RunSQL(sql);
                    bool flag = i <= -1 ? false : true;
                    return flag;
                }
                else //修改
                {
                    string sql = $"update SSQ set red1={obj.red1},red2={obj.red2},red3={obj.red3},red4={obj.red4},red5={obj.red5},red6={obj.red6},blue1={obj.blue1},time='{obj.time}' where expect like '{obj.expect}'";
                    int i = sqlite.RunSQL(sql);
                    bool flag = i <= -1 ? false : true;
                    return flag;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public SsqEntity GetInfo(string id)
        {
            try
            {
                string sql = $"select * from SSQ where expect like '{id}'";
                DataSet ds = new DataSet();
                sqlite.RunSQL(sql, ref ds);
                DataSetUtil<SsqEntity> util = new DataSetUtil<SsqEntity>();
                return util.DataSetToClassList(ds).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
