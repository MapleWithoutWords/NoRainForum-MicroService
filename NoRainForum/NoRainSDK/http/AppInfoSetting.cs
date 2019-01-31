using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NoRainSDK.http
{
    public class AppInfoSetting
    {
        private AppInfoSetting() { }
        [Required(ErrorMessage = "AppKey不能为空")]
        public string AppKey { get; set; }
        [Required(ErrorMessage = "AppSecret不能为空")]
        public string AppSecret { get; set; }
  
       

        public static AppInfoSetting Setting { get; set; } = new AppInfoSetting();
    }
}
