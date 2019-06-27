using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Dals
{
    public class AlarmmanagerDal
    {
        /// <summary>
        /// 得到报警参数配置
        /// </summary>
        /// <returns></returns>
        public AlarmConfigParms GetAlarmConfig()
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetAlarmConfig();
        }

        /// <summary>
        /// 配置报警参数
        /// </summary>
        /// <returns></returns>
        public bool SetAlarmConfig(AlarmConfigParms parms)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.SetAlarmConfig(parms);
        }

        #region 区域监控
        /// <summary>
        /// 得到区域监控列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<AreaMonitoringAlarm> GetAreaMonitoringAlarmList(InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetAreaMonitoringAlarmList(inPage);
        }
        /// <summary>
        /// 得到区域监控列表,根据机器人标识查询
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<AreaMonitoringAlarm> GetAreaMonitoringAlarmList(string tag, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetAreaMonitoringAlarmList(tag, inPage);
        }
        /// <summary>
        /// 得到区域监控列表,根据时间范围查询
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<AreaMonitoringAlarm> GetAreaMonitoringAlarmList(DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetAreaMonitoringAlarmList(stratTime, endTime, inPage);
        }
        /// <summary>
        /// 得到区域监控列表,根据时间范围查询和机器人标识查询
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<AreaMonitoringAlarm> GetAreaMonitoringAlarmList(string tag, DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetAreaMonitoringAlarmList(tag, stratTime, endTime, inPage);
        }
        /// <summary>
        /// 删除区域报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteAreaMonitoringAlarm(int[] ids)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.DeleteAreaMonitoringAlarm(ids);
        }
        #endregion

        #region 黑名单报警
        /// <summary>
        /// 得到黑名单报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<BlacklistAlarm> GetBlacklistAlarmList(InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetBlacklistAlarmList(inPage);
        }
        /// <summary>
        /// 得到黑名单报警列表,根据机器人标识查询
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<BlacklistAlarm> GetBlacklistAlarmList(string tag, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetBlacklistAlarmList(tag,inPage);
        }
        /// <summary>
        /// 得到黑名单报警列表,根据时间范围查询
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<BlacklistAlarm> GetBlacklistAlarmList(DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetBlacklistAlarmList(stratTime, endTime, inPage);
        }
        /// <summary>
        /// 得到黑名单报警列表,根据时间范围查询和机器人标识查询
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<BlacklistAlarm> GetBlacklistAlarmList(string tag, DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetBlacklistAlarmList(tag, stratTime, endTime, inPage);
        }
        /// <summary>
        /// 删除黑名单报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteBlacklistAlarm(int[] ids)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.DeleteBlacklistAlarm(ids);
        }
        #endregion

        #region 人脸报警
        /// <summary>
        /// 得到人脸报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<FaceContrastAlarm> GetFaceContrastAlarmList(InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetFaceContrastAlarmList(inPage);
        }
        /// <summary>
        /// 得到人脸报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<FaceContrastAlarm> GetFaceContrastAlarmList(string tag, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetFaceContrastAlarmList(tag, inPage);
        }
        /// <summary>
        /// 得到人脸报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<FaceContrastAlarm> GetFaceContrastAlarmList(DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetFaceContrastAlarmList(stratTime, endTime, inPage);
        }
        /// <summary>
        /// 得到人脸报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<FaceContrastAlarm> GetFaceContrastAlarmList(string tag, DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetFaceContrastAlarmList(tag, stratTime, endTime, inPage);
        }
        /// <summary>
        /// 删除人脸报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteFaceContrastAlarm(int[] ids)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.DeleteFaceContrastAlarm(ids);
        }
        #endregion

        #region 低温报警
        /// <summary>
        /// 得到低温报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<LowTemperatureAlarm> GetLowTemperatureAlarmList(InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetLowTemperatureAlarmList(inPage);
        }
        /// <summary>
        /// 得到低温报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<LowTemperatureAlarm> GetLowTemperatureAlarmList(string tag, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetLowTemperatureAlarmList(tag, inPage);
        }
        /// <summary>
        /// 得到低温报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<LowTemperatureAlarm> GetLowTemperatureAlarmList(DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetLowTemperatureAlarmList(stratTime, endTime, inPage);
        }
        /// <summary>
        /// 得到低温报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<LowTemperatureAlarm> GetLowTemperatureAlarmList(string tag, DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetLowTemperatureAlarmList(tag, stratTime, endTime, inPage);
        }
        /// <summary>
        /// 删除低温报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteLowTemperatureAlarm(int[] ids)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.DeleteLowTemperatureAlarm(ids);
        }
        #endregion

        #region 核辐射报警
        /// <summary>
        /// 得到核辐射报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<NuclearRadiationAlarm> GetNuclearRadiationAlarmList(InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetNuclearRadiationAlarmList(inPage);
        }
        /// <summary>
        /// 得到核辐射报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<NuclearRadiationAlarm> GetNuclearRadiationAlarmList(string tag, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetNuclearRadiationAlarmList(tag, inPage);
        }
        /// <summary>
        /// 得到核辐射报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<NuclearRadiationAlarm> GetNuclearRadiationAlarmList(DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetNuclearRadiationAlarmList(stratTime, endTime, inPage);
        }
        /// <summary>
        /// 得到核辐射报警列表
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<NuclearRadiationAlarm> GetNuclearRadiationAlarmList(string tag, DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetNuclearRadiationAlarmList(tag, stratTime, endTime, inPage);
        }
        /// <summary>
        /// 删除核辐射报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteNuclearRadiationAlarm(int[] ids)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.DeleteNuclearRadiationAlarm(ids);
        }
        #endregion

        #region 智能判图报警
        /// <summary>
        /// 智能判图报警
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<SmartFigureAlarm> GetSmartFigureAlarmList(InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetSmartFigureAlarmList(inPage);
        }

        /// <summary>
        /// 智能判图报警
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<SmartFigureAlarm> GetSmartFigureAlarmList(string tag, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetSmartFigureAlarmList(tag, inPage);
        }
        /// <summary>
        /// 智能判图报警
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<SmartFigureAlarm> GetSmartFigureAlarmList(DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetSmartFigureAlarmList(stratTime, endTime, inPage);
        }
        /// <summary>
        /// 智能判图报警
        /// </summary>
        /// <param name="inPage"></param>
        /// <returns></returns>
        public OutPage<SmartFigureAlarm> GetSmartFigureAlarmList(string tag, DateTime stratTime, DateTime endTime, InPage inPage)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.GetSmartFigureAlarmList(tag, stratTime, endTime, inPage);
        }
        /// <summary>
        /// 删除智能判图报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteSmartFigureAlarm(int[] ids)
        {
            AlarmManagement alarm = new AlarmManagement();
            return alarm.DeleteSmartFigureAlarm(ids);
        }
        #endregion

    }
}
