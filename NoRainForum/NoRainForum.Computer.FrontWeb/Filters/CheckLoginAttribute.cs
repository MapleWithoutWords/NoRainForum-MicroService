using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Filters
{
    [AttributeUsage( AttributeTargets.Method,AllowMultiple =true)]
    public class CheckLoginAttribute:Attribute
    {
    }
}
