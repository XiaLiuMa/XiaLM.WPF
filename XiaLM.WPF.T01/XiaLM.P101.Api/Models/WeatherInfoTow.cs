using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.P101.Api.Models
{
    public class WeatherInfoTow
    {
        public int error { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public ResultTow[] results { get; set; }
    }

    public class ResultTow
    {
        public string currentCity { get; set; }
        public string pm25 { get; set; }
        public Index[] index { get; set; }
        public Weather_Data[] weather_data { get; set; }
    }

    public class Index
    {
        public string des { get; set; }
        public string tipt { get; set; }
        public string title { get; set; }
        public string zs { get; set; }
    }

    public class Weather_Data
    {
        public string date { get; set; }
        public string dayPictureUrl { get; set; }
        public string nightPictureUrl { get; set; }
        public string weather { get; set; }
        public string wind { get; set; }
        public string temperature { get; set; }
    }
}
