using System;
using System.Collections.Generic;
using System.Text;

namespace Services.APICommon
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class ApiAuthorAttribute:Attribute
    {
    }
}
