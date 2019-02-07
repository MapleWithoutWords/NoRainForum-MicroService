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
using Newtonsoft.Json;
using NoRainForum.Computer.FrontWeb.Filters;
using NoRainForumCommon;
using NoRainSDK.src;

namespace NoRainForum.Computer.FrontWeb
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
            //使用iis托管
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379");

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            
            //将session存到redis
            services.AddSingleton<IDistributedCache>(
        serviceProvider =>
            new RedisCache(new RedisCacheOptions
            {
                Configuration = "127.0.0.1:6379",
                InstanceName = "FrontSample:"
            }));
            //注册服务
            RegisterService(services);
            //添加对session的支持
            services.AddSession();
            services.AddSingleton(typeof(NoRainExceptionFilter));
            services.AddMvc(options=> {
                var serviceProvider = services.BuildServiceProvider();
                var exceptionFilter = serviceProvider.GetService<NoRainExceptionFilter>();
                options.Filters.Add(exceptionFilter);
                options.Filters.Add(new NoRainActionFilter());
                options.Filters.Add(new NoRainFrontAuthorizatinFilter());
            })
            .AddJsonOptions(options =>
            {
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private static void RegisterService(IServiceCollection services)
        {
            services.AddSingleton(new AdminUserService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new RoleService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PermissionService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostStatusService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostTypeService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new UserService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new PostCommentService(SettingModel.AppKey, SettingModel.AppSecret));
            services.AddSingleton(new SendEmailService(SettingModel.AppKey, SettingModel.AppSecret));
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            app.UseCookiePolicy();
        }
    }
}
