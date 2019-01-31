using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.APICommon;
using Services.Common.IServiceCommon;

namespace Services.User.API
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
            services.AddMvc(option=> {
                option.Filters.Add(new APIResultFilter());
                //option.Filters.Add(new APIAuthorizationFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            Register(services, Assembly.Load("Services.User.Service"));
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //注册到Consul
            RegisterConsul.Register(Configuration, "UserService").Wait();
        }
    }
}
