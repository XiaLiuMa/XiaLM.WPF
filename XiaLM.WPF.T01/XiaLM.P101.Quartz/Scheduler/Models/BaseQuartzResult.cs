﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XiaLM.P101.Quartz.Scheduler.Models
{
    public class BaseQuartzResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }
}
