using Newtonsoft.Json;
using System;
using XiaLM.Log;
using XiaLM.Owin.UserAttributes;
using XiaLM.WPF.T01.Net.Hubs;
using XiaLM.WPF.T01.Net.Models;

namespace XiaLM.WPF.T01.Net.Manaments
{
    public class AlarmManamentController : ApiController
    {
        /// <summary>
        /// 推送报警消息
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        [HttpPost]
        public bool PushAlarmInfo(string alarm)
        {
            try
            {
                AlarmInfo data = JsonConvert.DeserializeObject<AlarmInfo>(alarm);
                if (data == null) return false;
                AlarmHub.GetInstance().PushAlarmInfo(data);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return false;
        }
    }
}
