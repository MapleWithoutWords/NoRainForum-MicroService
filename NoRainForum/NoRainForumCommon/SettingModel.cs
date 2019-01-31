using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForumCommon
{
    public static class SettingModel
    {
 
        public static string AppKey { get; set; } = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("AppInfo").GetSection("AppKey").Value;
        public static string AppSecret { get; set; } = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("AppInfo").GetSection("AppSecret").Value;
    }
}
