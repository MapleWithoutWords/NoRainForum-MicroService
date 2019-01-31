using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services.User.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            string ip = config["ip"];
            string port = config["port"];
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port))
            {
                ip = "127.0.0.1";
                port = "6000";	//设置一个没有被占用的端口
            }
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://{ip}:{port}")
                .UseStartup<Startup>();
        }
    }
}
