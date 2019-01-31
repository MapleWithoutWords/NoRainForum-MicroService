using System;
using System.Collections.Generic;
using System.Text;

namespace Services.APICommon
{
    public class APIResult<T>
    {
        public string ErrorMsg { get; set; }
        public T Data { get; set; }
    }
}
