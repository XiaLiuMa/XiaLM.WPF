using System;
using System.Collections.Generic;
using System.Text;

namespace XiaLM.P101.Quartz.Scheduler.Models
{
    /// <summary>
	/// 任务调度详情
	/// </summary>
	public class ScheduleDetails
    {
        ///// <summary>
        ///// 任务分组
        ///// </summary>
        //public string JobGroup { get; set; }
        ///// <summary>
        ///// 任务名称
        ///// </summary>
        //public string JobName { get; set; }
        ///// <summary>
        ///// 方法描述
        ///// </summary>
        //public string ActionDescribe { get; set; }
        ///// <summary>
        ///// 任务执行状态
        ///// </summary>
        //public EnumType.JobStep ActionStep { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }
        ///// <summary>
        ///// 是否成功执行
        ///// </summary>
        //public int IsSuccess { get; set; }


        /// <summary>
        /// 任务编号
        /// </summary>
        public Guid Id { get; set; }
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
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int RunTimes { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 停止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 外部地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public EnumType.JobStatus Status { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Agrs { get; set; }



    }
}
