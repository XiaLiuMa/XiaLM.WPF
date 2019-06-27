using MaxRobotServerApp.Dals;
using MaxRobotServerApp.Mods.Config;
using MaxRobotServerApp.Views.Flyouts;
using Maxvision.Robot.Sdk;
using Maxvision.Robot.Sdk.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxRobotServerApp.Extensions.Comm
{
    /// <summary>
    /// 配置文件管理器
    /// </summary>
    public class ConfigManager
    {
        #region 单例
        private static ConfigManager instance;
        private readonly static object objLock = new object();

        public static ConfigManager GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigManager();
                    }
                }
            }
            return instance;
        }
        #endregion

        public SystemConfig LoadSystemConfig()
        {
            try
            {
                string fpath = AppDomain.CurrentDomain.BaseDirectory + @"config\SystemConfig.json";
                string json = File.ReadAllText(fpath);
                SystemConfig config = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemConfig>(json);
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool SaveSystemConfig(SystemConfig config)
        {
            try
            {
                string fpath = AppDomain.CurrentDomain.BaseDirectory + @"config\SystemConfig.json";
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(config);
                File.WriteAllText(fpath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
