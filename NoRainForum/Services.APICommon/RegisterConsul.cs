using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.APICommon
{
    public class RegisterConsul
    {
        /// <summary>
        /// 服务的名字
        /// </summary>
        /// <param name="Configuration"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static async Task Register(IConfiguration Configuration,string serviceName)
        {
            string ip = Configuration["ip"];    //获取本接口所在服务器的ip
            //获取本接口所在服务器的ip的端口
            int port = Convert.ToInt32(Configuration["port"]);
            //服务的id，不可重复。
            string serviceId = serviceName + Guid.NewGuid();
            //创建一个consul客户端
            using (var client = new ConsulClient(ConsulConfig))
            {
                //将本接口注册到consul
                await client.Agent
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
                      });
            }
        }

        private static void ConsulConfig(ConsulClientConfiguration c)
        {
            c.Address = new Uri("http://127.0.0.1:8500");   //consul所在的服务器
            c.Datacenter = "dc1";           //设置数据中心的名称
        }
    }
}
