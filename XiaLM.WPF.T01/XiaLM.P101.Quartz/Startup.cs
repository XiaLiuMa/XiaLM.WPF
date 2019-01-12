using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XiaLM.P101.Quartz.Hubs;

namespace XiaLM.P101.Quartz
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            //builder.AddEnvironmentVariables();
            //Configuration = builder.Build();

            //BaseMapper.Initialize();    //初始化映射关系
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法将服务添加到容器中。
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");//获取数据库连接字符串
            //services.AddDbContext<BaseDBContext>(options => options.UseSqlServer(sqlConnectionString));

            ////依赖注入
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IMenuRepository, MenuRepository>();
            //services.AddScoped<IMenuService, MenuService>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IDepartmentService, DepartmentService>();
            //services.AddScoped<IRoleRepository, RoleRepository>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddMvc();

            //services.AddSession();  //Session服务
            services.AddSignalR();
        }

        ///// <summary>
        ///// 此方法由运行时调用。使用此方法配置HTTP请求管道
        ///// </summary>
        ///// <param name="app"></param>
        ///// <param name="env"></param>
        ///// <param name="loggerFactory"></param>
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    //loggerFactory.AddConsole();

        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();//开发环境异常处理
        //    }

        //    app.UseOwin(x => x.UseNancy());
        //}

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());

            app.UseSignalR(route =>
            {
                route.MapHub<MyChatHub>("/myChathub");
            });

        }
    }
}
