using Newtonsoft.Json;
using System;
using System.IO;

namespace XiaLM.P101.Api
{
    public class MainRealize
    {
        public Config _Config;
        private static MainRealize instance;
        private readonly static object objLock = new object();
        public static MainRealize GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new MainRealize();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public void InitConfig()
        {
            string configStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Config.json");
            _Config = JsonConvert.DeserializeObject<Config>(configStr);
        }
    }
}
