using Newtonsoft.Json;
using System;
using XiaLM.Log;
using XiaLM.Owin.UserAttributes;
using XiaLM.P101.Api.Models;
using XiaLM.P101.Api.Realizes;

namespace XiaLM.P101.Api.Controllers
{
    public class WeatherController : ApiController
    {
        /// <summary>
        /// 获取天气信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public WeatherInfo GetWeatherInfo(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                param = WeatherRealize.GetInstance().GetCurrentCity();
            }
            return WeatherRealize.GetInstance().GetWeather(param);
        }
    }
}
