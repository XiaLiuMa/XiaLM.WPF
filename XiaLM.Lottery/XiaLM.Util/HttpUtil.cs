using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Util
{
    public class HttpUtil
    {
        private HttpClient http;
        private int timeOut = 1000 * 60 * 5;//超时
        /// <summary>
        /// 超时
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set
            {
                timeOut = value;
                http.Timeout = new TimeSpan(0, 0, 0, 0, timeOut);
            }
        }
        public HttpUtil()
        {
            http = new HttpClient();
            http.Timeout = new TimeSpan(0, 0, 0, 0, timeOut);
        }

        #region 实现IHttpMethod接口
        public string Get<T>(string url, T t)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    url = url + getHttpGetParms(t);
                    using (var response = http.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception)
                {

                }
            }).Wait(timeOut);
            return result;
        }

        public string Get(string url)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var response = http.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception ex)
                {

                }
            }).Wait(timeOut);
            return result;
        }
        public byte[] GetBytes(string url)
        {
            byte[] result = null;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var response = http.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsByteArrayAsync().Result;
                    }
                }
                catch (Exception ex)
                {

                }
            }).Wait(timeOut);
            return result;
        }
        public byte[] GetBytes<T>(string url, T t)
        {
            byte[] result = null;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    url = url + getHttpGetParms(t);
                    using (var response = http.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsByteArrayAsync().Result;
                    }
                }
                catch (Exception ex)
                {

                }
            }).Wait(timeOut);
            return result;
        }
        public string PostToJson<T>(string url, T t)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                    var content = new StringContent(json, Encoding.UTF8);
                    content.Headers.ContentType.MediaType = "application/json";
                    using (var response = http.PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception)
                {

                }
            }).Wait(timeOut);
            return result;
        }
        public string Post<T>(string url, T t)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var content = new FormUrlEncodedContent(getHttpPostParms(t));
                    content.Headers.ContentType.MediaType = "application/json";
                    using (var response = http.PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception)
                {

                }
            }).Wait(timeOut);
            return result;
        }

        public string Post(string url)
        {
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>());
                    using (var response = http.PostAsync(url, content).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception)
                {

                }
            }).Wait(timeOut);
            return result;
        }

        #endregion

        #region 私有方法
        private Dictionary<string, string> getHttpPostParms<T>(T t)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (t != null)
            {
                Type type = t.GetType();
                var p = type.GetProperties();
                foreach (var item in p)
                {
                    dic.Add(item.Name, item.GetValue(t).ToString());
                }
            }
            return dic;
        }
        private string getHttpGetParms<T>(T t)
        {
            string parms = string.Empty;
            if (t != null)
            {
                Type type = t.GetType();
                var p = type.GetProperties();
                foreach (var item in p)
                {
                    parms += item.Name + "=" + item.GetValue(t).ToString() + "&";
                }
                parms = "?" + parms.TrimEnd('&');
            }
            return parms;
        }
        #endregion
    }
}
