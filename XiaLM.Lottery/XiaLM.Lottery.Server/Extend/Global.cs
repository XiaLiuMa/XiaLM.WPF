using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaLM.Lottery.Server.Dals;
using XiaLM.Lottery.Server.Entities;
using XiaLM.Lottery.Server.Mods;
using XiaLM.Util;

namespace XiaLM.Lottery.Server.Extend
{
    public class Global
    {
        /// <summary>
        /// 数据同步
        /// </summary>
        public static void DataSynchronism()
        {

            Task.Factory.StartNew(() =>
            {
                string api = $@"http://www.mxnzp.com/api/lottery/ssq/lottery_list?page=1";
                string resultJson = new HttpUtil().Post(api);
                var obj = JsonUtil.StrToObject<ApiSsqSelectResult>(resultJson);
                if (obj.data != null && obj.data.totalPage > 0)
                {
                    for (int i = 1; i <= obj.data.totalPage; i++)
                    {
                        string tapi = $@"http://www.mxnzp.com/api/lottery/ssq/lottery_list?page={i}";
                        string tjson = new HttpUtil().Post(api);
                        var tobj = JsonUtil.StrToObject<ApiSsqSelectResult>(resultJson);
                        foreach (var item in tobj.data.list)
                        {
                            string[] tstrs = item.openCode.Split(',', '+');
                            SsqEntity enty = new SsqEntity()
                            {
                                expect = item.expect,
                                red1 = Convert.ToInt32(tstrs[0]),
                                red2 = Convert.ToInt32(tstrs[1]),
                                red3 = Convert.ToInt32(tstrs[2]),
                                red4 = Convert.ToInt32(tstrs[3]),
                                red5 = Convert.ToInt32(tstrs[4]),
                                red6 = Convert.ToInt32(tstrs[5]),
                                blue1 = Convert.ToInt32(tstrs[6]),
                                time = item.time.Split(' ')[0]
                            };

                            bool flag = new SsqDal().RefreshData(enty);
                            Console.WriteLine($"{i}:{item.expect}:{flag}");
                        }
                        Thread.Sleep(2 * 1000);    //10秒钟请求一次
                    }
                }
            });
        }
    }
}
