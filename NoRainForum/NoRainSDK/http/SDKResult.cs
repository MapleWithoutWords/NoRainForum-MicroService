using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NoRainSDK.http
{
    public class SDKResult
    {
        public string Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
