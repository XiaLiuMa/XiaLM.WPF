using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace XiaLM.Lottery.Server.Extend
{
    //public class WebServer : IDisposable
    //{
    //    private IWebHost webHost;

    //    public void Start()
    //    {
    //        Task.Factory.StartNew(() =>
    //        {
    //            try
    //            {
    //                if (PortInUse(ConfigHandler.GetInstance().baseConfig.HostPort))    //端口被占用
    //                {
    //                    int tempPort = 0;
    //                    Random rand = new Random();
    //                    do
    //                    {
    //                        tempPort = rand.Next(5000, 10000);
    //                    } while (PortInUse(tempPort));
    //                    ConfigHandler.GetInstance().baseConfig.HostPort = tempPort;
    //                    ConfigHandler.GetInstance().SaveBaseConfig(ConfigHandler.GetInstance().baseConfig);
    //                    Thread.Sleep(1000); //等待基础数据刷新
    //                }
    //                string webapiUrl = $"http://{ConfigHandler.GetInstance().baseConfig.HostIp}:{ConfigHandler.GetInstance().baseConfig.HostPort}";

    //                webHost = new WebHostBuilder()
    //                    .UseKestrel()
    //                    .UseUrls(webapiUrl)
    //                    //.UseWebRoot($"{AppContext.BaseDirectory}/wwwroot")
    //                    .UseContentRoot(AppContext.BaseDirectory)
    //                    .UseIISIntegration()
    //                    .UseStartup<Startup>()
    //                    .Build();
    //                webHost.Run();
    //            }
    //            catch (Exception ex)
    //            {
    //                Log.Info("关闭WebServer失败。");
    //                Log.Error(ex.ToString());
    //            }
    //        });
    //    }

    //    /// <summary>
    //    /// 检测端口是否被占用
    //    /// </summary>
    //    /// <param name="port"></param>
    //    /// <returns></returns>
    //    private bool PortInUse(int port)
    //    {
    //        IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
    //        IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
    //        return ipEndPoints.Any(c => c.Port == port);
    //    }

    //    public void Dispose()
    //    {
    //        Log.Info("关闭WebServer...");
    //        webHost?.StopAsync();
    //        webHost?.Dispose();
    //    }
    //}

    //public class Startup
    //{
    //    /// <summary>
    //    /// 此方法由运行时调用。使用此方法将服务添加到容器中。
    //    /// </summary>
    //    /// <param name="services"></param>
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddMvc();

    //        //添加cors 服务
    //        //services.AddCors(options =>options.AddPolicy("CorsSample", p => p.WithOrigins("http://192.168.110.56:5539").AllowAnyMethod().AllowAnyHeader()));
    //    }

    //    /// <summary>
    //    /// 此方法由运行时调用。使用此方法配置HTTP请求管道
    //    /// </summary>
    //    /// <param name="app"></param>
    //    /// <param name="env"></param>
    //    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    //    {
    //        #region 调用文件服务器，使用静态html文件
    //        app.UseStaticFiles();//缺少会导致wwwroot下的资源无法访问
    //        app.UseStaticFiles(new StaticFileOptions()
    //        {
    //            FileProvider = new PhysicalFileProvider($@"{AppContext.BaseDirectory}wwwroot"),
    //            RequestPath = "/wwwroot"
    //        });
    //        #endregion

    //        app.UseMiddleware<MvcHandlerMiddleware>();  //注册路由拦截中间件
    //        app.UseMvc(routes =>
    //        {
    //            routes.MapRoute(
    //                name: "default",
    //                template: "{controller=Home}/{action=Index}/{id?}");
    //        });//使用Mvc，设置默认路由为系统登录


    //        //app.UseCors("CorsSample");  //配置Cors
    //    }
    //}

    ///// <summary>
    ///// MVC路由拦截中间件
    ///// </summary>
    //public class MvcHandlerMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public MvcHandlerMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
    //    {
    //        if (Global.cache.Get("user") == null)   //缓存中没有用户
    //        {
    //            if (httpContext.Request.Path == "/login/CkLogin")
    //            {
    //                return _next(httpContext);  //放行登陆请求
    //            }
    //            else
    //            {
    //                var app = new ApplicationBuilder(serviceProvider);
    //                #region 废弃
    //                //app.UseMvc(routes =>
    //                //{
    //                //    routes.MapRoute(
    //                //        "mvchandler",
    //                //        "mvchandler",
    //                //        new { Controller = "Login", Action = "Index" });
    //                //});
    //                //app.Build().Invoke(httpContext);
    //                //return Task.CompletedTask; 
    //                #endregion
    //                httpContext.Request.Path = "/login/CkLogin";
    //                app.Build().Invoke(httpContext);
    //                return Task.CompletedTask;
    //            }
    //        }

    //        return _next(httpContext);
    //    }
    //}
}
