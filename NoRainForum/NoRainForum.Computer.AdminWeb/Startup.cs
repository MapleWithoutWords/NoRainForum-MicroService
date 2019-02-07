using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NoRainForum.Computer.AdminWeb.filters;
using NoRainForum.Computer.AdminWeb.Models;
using NoRainForumCommon;
using NoRainSDK.src;

namespace NoRainForum.Computer.AdminWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            services.AddSingleton<IDistributedCache>(
             serviceProvider =>
                 new RedisCache(new RedisCacheOptions
                 {
                     Configuration = "127.0.0.1:6379",
                     InstanceName = "AdminSample:"
                 }));
            //注册服务
            RegisterService(services);
            //添加对session的支持
            services.AddSession();
            //添加权限验证
            services.AddSingleton(typeof(NoRainAuthorizationFilter));
            services.AddSingleton(typeof(NoRainExceptionFilter));
            services.AddMvc(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var obj = serviceProvider.GetService<NoRainAuthorizationFilter>();
                var exceptionFilter = serviceProvider.GetService<NoRainExceptionFilter>();
                options.Filters.Add(obj);
                options.Filters.Add(exceptionFilter);
                services.AddSingleton(new NoRainActionFilter());
            })
            .AddJsonOptions(options =>
            {
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private static void RegisterService(IServiceCollection services)
        {
            services.AddSingleton(new AdminUserService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new RoleService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PermissionService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostStatusService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostTypeService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new SettingService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new UserService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostCommentService(SettingModel.AppKey, SettingModel.AppSecret));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Error404");
            }

            app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });
            app.UseStaticFiles();
            app.UseCookiePolicy();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
        }
    }
}
