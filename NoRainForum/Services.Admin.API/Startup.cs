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
                option.Filters.Add(new APIExceptionFilter());
                //option.Filters.Add(new APIAuthorizationFilter());    
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

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
            string ip = "127.0.0.1";    //获取本接口所在服务器的ip
            //获取本接口所在服务器的ip的端口
            int port = 2001;
            string serviceName = "AdminService";
            //服务的id，不可重复。
            string serviceId = serviceName + Guid.NewGuid();
            //创建一个consul客户端
            using (var client = new ConsulClient(ConsulConfig))
            {
                //将本接口注册到consul
                 client.Agent
                      .ServiceRegister(new AgentServiceRegistration()
                      {
                          Address = ip,
                          ID = serviceId,
                          Name = serviceName,
                          Port = port,
                          Check = new AgentServiceCheck()
                          {
                              //服务停止多久后反注册(注销)
                              DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                              //健康检查时间间隔，或者称为心跳间隔
                              Interval = TimeSpan.FromSeconds(10),
                              //健康检查地址，Healthy：用于健康检查的接口
                              HTTP = $"http://{ip}:{port}/api/Healthy",
                              //设置超时时间
                              Timeout = TimeSpan.FromSeconds(5)
                          }
                      }).Wait();
            }

        }
        private static void ConsulConfig(ConsulClientConfiguration c)
        {
            c.Address = new Uri("http://127.0.0.1:8500");   //consul所在的服务器
            c.Datacenter = "dc1";           //设置数据中心的名称
        }
    }
}
