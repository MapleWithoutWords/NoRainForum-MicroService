using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : Attribute
    {
        public string PermissionName { get; set; }
        public CheckPermissionAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
