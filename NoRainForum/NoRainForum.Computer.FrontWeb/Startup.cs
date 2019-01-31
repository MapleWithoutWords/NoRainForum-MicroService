using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            RegisterService(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
