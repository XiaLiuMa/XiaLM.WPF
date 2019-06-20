using System;
using System.Collections.Generic;
using System.Text;

namespace XiaLM.Lottery.Server.Mods
{
    public class ApiSsqSelectResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public SsqSelectResult data { get; set; }
    }

    public class SsqSelectResult
    {
        public int page { get; set; }  //"1"
        public int totalCount { get; set; }    //"903"
        public int totalPage { get; set; }    //"91"
        public int limit { get; set; }    //"91"

        public List<SsqSelectResult_List> list { get; set; }
    }

    public class SsqSelectResult_List
    {
        public string openCode { get; set; }  //"10,12,15,25,26,27+14"
        public string code { get; set; }    //"ssq"
        public string expect { get; set; }    //"2018136"
        public string name { get; set; }    //"双色球"
        public string time { get; set; }    //"2018-11-20 21:18:20"
    }
}
