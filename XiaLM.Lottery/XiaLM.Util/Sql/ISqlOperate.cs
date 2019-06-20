using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Lottery.Server.Extend.Sql
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    public interface ISqlOperate : IDisposable
    {
        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        string DbConStr { get; set; }

        /// <summary>
        /// 執行SQL，主要用於數據插入，可以拋出異常
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        int RunSQL(string cmdText);

        /// <summary>
        /// 执行SQL语句命令
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="dataSet">返回DataSet对象</param>
        void RunSQL(string cmdText, ref DataSet ds);
    }
}
