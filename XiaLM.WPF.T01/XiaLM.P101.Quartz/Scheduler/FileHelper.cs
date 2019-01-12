using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XiaLM.P101.Quartz.Scheduler
{
    /// <summary>
    /// 文件相关助手类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 反射获取类信息
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Type GetAbsolutePath(string assemblyName, string className)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
            Type type = assembly.GetType(className);
            return type;
        }
    }
}
