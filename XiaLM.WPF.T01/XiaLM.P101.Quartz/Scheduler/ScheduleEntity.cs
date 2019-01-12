﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XiaLM.P101.Quartz.Scheduler
{
    /// <summary>
    /// 任务调度实体
    /// </summary>
    public class ScheduleEntity
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 执行周期表达式
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 外部地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public EnumType.JobStatus Status { get; set; }
        /// <summary>
        /// 任务运行状态
        /// </summary>
        public EnumType.JobRunStatus RunStatus { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 停止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunTimes { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Agrs { get; set; }
    }
}
