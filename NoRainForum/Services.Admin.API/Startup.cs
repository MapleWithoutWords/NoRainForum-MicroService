using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Admin.API.Filters;
using Services.APICommon;
using Services.Common.IServiceCommon;

namespace Services.Admin.API
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
            services.AddMvc(option =>
            {
                option.Filters.Add(new APIResultFilter());
                //option.Filters.Add(new APIAuthorizationFilter());    
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1); 

            Register(services, Assembly.Load("Services.Admin.Service"));

        }
        private void Register(IServiceCollection services, Assembly assem)
        {
            var types = assem.GetTypes().Where(e => !e.IsAbstract && typeof(ISupport).IsAssignableFrom(e));
            foreach (var type in types)
            {
                foreach (var inter in type.GetInterfaces())
                {
                    services.AddSingleton(inter, type);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            RegisterConsul.Register(Configuration, "AdminService").Wait();

        }
    }
}
