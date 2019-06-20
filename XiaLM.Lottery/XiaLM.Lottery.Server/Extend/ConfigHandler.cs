using System;
using System.Collections.Generic;
using System.Text;

namespace XiaLM.Lottery.Server.Extend
{
    ///// <summary>
    ///// 配置文件管理工厂
    ///// </summary>
    //public class ConfigHandler
    //{
    //    #region 单例
    //    private static ConfigHandler instance;
    //    private readonly static object objLock = new object();
    //    public static ConfigHandler GetInstance()
    //    {
    //        if (instance == null)
    //        {
    //            lock (objLock)
    //            {
    //                if (instance == null)
    //                {
    //                    instance = new ConfigHandler();
    //                }
    //            }
    //        }
    //        return instance;
    //    }
    //    #endregion

    //    private IFileProvider fileProvider;
    //    private string appConfigPath = $@"{AppContext.BaseDirectory}/config/";//app根路径
    //    private string baseConfigPath = @"BaseConfig.xml";//基础配置
    //    private string processConfigPath = @"ProcessConfig.xml";//进程配置

    //    public BaseConfig baseConfig;
    //    /// <summary>
    //    /// 进程配置列表
    //    /// </summary>
    //    public List<ProcessConfig> processConfigs;


    //    /// <summary>
    //    /// 初始化
    //    /// </summary>
    //    public void Init()
    //    {
    //        fileProvider = new PhysicalFileProvider(appConfigPath);
    //        ChangeToken.OnChange(() => fileProvider.Watch(baseConfigPath), () => OnBaseChangedCallback(fileProvider));
    //        ChangeToken.OnChange(() => fileProvider.Watch(processConfigPath), () => OnProcessChangedCallback(fileProvider));

    //        #region 初始化配置文件
    //        baseConfig = XmlUtils.Load<BaseConfig>(appConfigPath + baseConfigPath);
    //        processConfigs = XmlUtils.Load<List<ProcessConfig>>(appConfigPath + processConfigPath);
    //        #endregion
    //    }

    //    /// <summary>
    //    /// 保存基础配置
    //    /// </summary>
    //    /// <param name="config"></param>
    //    public void SaveBaseConfig(BaseConfig config)
    //    {
    //        try
    //        {
    //            XmlUtils.Save(config, appConfigPath + baseConfigPath);
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error(ex.ToString());
    //        }
    //    }

    //    /// <summary>
    //    /// 基础配置文件修改事件
    //    /// </summary>
    //    /// <param name="state"></param>
    //    private void OnBaseChangedCallback(object state)
    //    {
    //        baseConfig = XmlUtils.Load<BaseConfig>(appConfigPath + baseConfigPath);
    //    }

    //    /// <summary>
    //    /// 进程配置文件修改事件
    //    /// </summary>
    //    /// <param name="state"></param>
    //    private void OnProcessChangedCallback(object state)
    //    {
    //        processConfigs = XmlUtils.Load<List<ProcessConfig>>(appConfigPath + processConfigPath);
    //    }

    //}
}
