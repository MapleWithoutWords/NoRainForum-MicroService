using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Admin.API.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class LoginAttribute : Attribute
    {
        public string PermissionName { get; set; }
        public LoginAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
