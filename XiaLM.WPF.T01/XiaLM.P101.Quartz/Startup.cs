using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using XiaLM.P101.Quartz.Db;
using XiaLM.P101.Quartz.Db.IManaments;
using XiaLM.P101.Quartz.Db.Manaments;
using XiaLM.P101.Quartz.App.Hubs;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace XiaLM.P101.Quartz
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            BaseMapper.Initialize();    //初始化映射关系
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法将服务添加到容器中。
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");//获取数据库连接字符串
            //services.AddDbContext<BaseDBContext>(options => options.UseSqlServer(sqlConnectionString));
            services.AddSignalR();
            //依赖注入
            services.AddScoped<IScheduleManament, ScheduleManament>();
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            });//使用静态文件

            app.UseOwin(x => x.UseNancy());
            app.UseSignalR(route =>
            {
                route.MapHub<HomeHub>("/homeHub");
            });
            SeedData.Initialize(app.ApplicationServices); //初始化数据
        }
    }
}
