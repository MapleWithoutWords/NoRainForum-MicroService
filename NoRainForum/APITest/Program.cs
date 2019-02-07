using System;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using NoRainSDK.src;
using System.Text.RegularExpressions;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri("http://127.0.0.1:8888/AdminService/api/AdminUser");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
