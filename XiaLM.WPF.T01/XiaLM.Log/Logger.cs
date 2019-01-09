using System;
using System.Diagnostics;
using System.IO;

namespace XiaLM.Log
{
    /// <summary>
    /// 日志助手
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 输出INFO日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("INFO");
            log.Info(msg);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "INFO",
                    Message = msg,
                    Exception = null,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出Warn日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>

        public static void Warn(string msg)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("WARN");
            log.Warn(msg);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "WARN",
                    Message = msg,
                    Exception = null,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出Error日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("ERROR");
            log.Error(msg);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "ERROR",
                    Message = msg,
                    Exception = null,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出Error日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(Exception ex, string msg = null)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("ERROR");
            log.Error(msg, ex);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "ERROR",
                    Message = msg,
                    Exception = ex,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出Fatal日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        public static void Fatal(string msg)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("FATAL");
            log.Fatal(msg);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "FATAL",
                    Message = msg,
                    Exception = null,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出Fatal日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        public static void Fatal(Exception ex, string msg = null)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("FATAL");
            log.Fatal(msg, ex);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "FATAL",
                    Message = msg,
                    Exception = ex,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出DEBUG日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("DEBUG");
            log.Debug(msg);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "DEBUG",
                    Message = msg,
                    Exception = null,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }

        /// <summary>
        /// 输出DEBUG日志到LOG4NET
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Debug(Exception ex, string msg = null)
        {
            if (!LogOutPut.GetInstance().CanWriteLog) return;
            log4net.ILog log = log4net.LogManager.GetLogger("DEBUG");
            log.Debug(msg, ex);
            if (LogOutPut.GetInstance().CanSendLog)
            {
                LogOutPut.GetInstance().Send(new OutMsg()
                {
                    Client = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName),
                    Type = "DEBUG",
                    Message = msg,
                    Exception = ex,
                    Time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")
                });
            }
        }
    }
}
