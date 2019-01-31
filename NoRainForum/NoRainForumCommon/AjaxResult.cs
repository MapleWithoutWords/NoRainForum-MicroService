using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainForumCommon
{
   public class AjaxResult
    {
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
        public object Data { get; set; }
    }
}
