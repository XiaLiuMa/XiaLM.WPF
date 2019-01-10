using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XiaLM.Log;
using XiaLM.P101.Api.Models;
using XiaLM.Utility;

namespace XiaLM.P101.Api.Realizes
{
    public class WeatherRealize
    {
        private static WeatherRealize instance;
        private readonly static object objLock = new object();
        public static WeatherRealize GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new WeatherRealize();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 获取当前城市
        /// </summary>
        /// <returns></returns>
        public string GetCurrentCity()
        {


            return "武汉";
        }

        /// <summary>
        /// 获取天气
        /// </summary>
        /// <param name="city">城市</param>
        /// <returns></returns>
        public WeatherInfo GetWeather(string city)
        {
            try
            {
                string key = MainRealize.GetInstance()._Config.Weather_ApiKey;
                string language = "zh-Hans";
                string requestUrl = $"https://api.seniverse.com/v3/weather/now.json?key={key}&location={city}&language={language}&unit=c";
                var response = HttpHelper.Get(requestUrl);
                var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(response);
                if (weatherInfo == null)
                {
                    return GetWeatherTow(city);
                }
                return weatherInfo;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 备用查询天气方案(心知天气免费api无法查询地级市以下的，备用方案却可以查询)
        /// </summary>
        /// <param name="city">城市</param>
        /// <returns></returns>
        private WeatherInfo GetWeatherTow(string city)
        {
            try
            {
                string requestUrl = $"http://api.jirengu.com/getWeather.php?city={city}";
                var response = HttpHelper.Get(requestUrl);
                var weatherInfoTow = JsonConvert.DeserializeObject<WeatherInfoTow>(response);
                if (weatherInfoTow == null) return null;
                string tempStr = weatherInfoTow.results[0].weather_data[0].date;
                Regex reg = new Regex(@"实时：(.+)℃\)");
                Match match = reg.Match(tempStr);
                string value = match.Groups[1].Value;
                return new WeatherInfo()
                {
                    results = new Result[] {
                    new Result(){
                        location = new Location(){
                            name = weatherInfoTow.results[0].currentCity
                        },
                        now = new Now(){
                            temperature = value,
                            text = weatherInfoTow.results[0].weather_data[0].weather,
                            code = GetWeatherCode(weatherInfoTow.results[0].weather_data[0].weather)
                        },
                    }
                }
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        private string GetWeatherCode(string weather)
        {
            if (weather.Equals("晴")) return "0";
            if (weather.Equals("多云")) return "4";
            if (weather.Equals("晴间多云")) return "5";
            if (weather.Equals("大部多云")) return "8";
            if (weather.Equals("阴")) return "9";
            if (weather.Equals("阵雨")) return "10";
            if (weather.Equals("雷阵雨")) return "11";
            if (weather.Equals("雷阵雨伴有冰雹")) return "12";
            if (weather.Equals("小雨")) return "13";
            if (weather.Equals("中雨")) return "14";
            if (weather.Equals("大雨")) return "15";
            if (weather.Equals("暴雨")) return "16";
            if (weather.Equals("大暴雨")) return "17";
            if (weather.Equals("特大暴雨")) return "18";
            if (weather.Equals("冻雨")) return "19";
            if (weather.Equals("雨夹雪")) return "20";
            if (weather.Equals("阵雪")) return "21";
            if (weather.Equals("小雪")) return "22";
            if (weather.Equals("中雪")) return "23";
            if (weather.Equals("大雪")) return "24";
            if (weather.Equals("暴雪")) return "25";
            if (weather.Equals("浮尘")) return "26";
            if (weather.Equals("扬沙")) return "27";
            if (weather.Equals("沙尘暴")) return "28";
            if (weather.Equals("强沙尘暴")) return "29";
            if (weather.Equals("雾")) return "30";
            if (weather.Equals("霾")) return "31";
            if (weather.Equals("风")) return "32";
            if (weather.Equals("大风")) return "33";
            if (weather.Equals("飓风")) return "34";
            if (weather.Equals("热带风暴")) return "35";
            if (weather.Equals("龙卷风")) return "36";
            if (weather.Equals("冷")) return "37";
            if (weather.Equals("热")) return "38";
            return "99";
        }


    }
}
