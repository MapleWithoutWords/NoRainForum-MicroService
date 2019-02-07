using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NoRainForum.Computer.AdminWeb.common;
using NoRainForumCommon;
using NoRainSDK.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.AdminWeb.filters
{
    public class NoRainAuthorizationFilter : IAuthorizationFilter
    {
        public PermissionService PerSvc { get; set; }
        public NoRainAuthorizationFilter(PermissionService PerSvc)
        {
            this.PerSvc = PerSvc;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            CheckPermissionAttribute[] pers = (CheckPermissionAttribute[])
                  ((ControllerActionDescriptor)context.ActionDescriptor)
                  .MethodInfo.GetCustomAttributes(typeof(CheckPermissionAttribute), false);
            if (pers.Length < 1)
            {
                return;
            }

            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString(ConstList.ADMINUSERID)))
            {
                if (context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    context.Result = new RedirectResult("/home/Login");
                    return;
                }
                context.Result = new JsonResult(new AjaxResult { Status = "redirect", Data = "/home/Login" });
                return;
            }
            long id = Convert.ToInt64(context.HttpContext.Session.GetString(ConstList.ADMINUSERID));
            foreach (var attr in pers)
            {
                CheckPermissionModel model = new CheckPermissionModel();
                model.AdminUserId = id;
                model.PermissionName = attr.PermissionName;
                if (!(await PerSvc.CheckPermissionAsync(model)))
                {
                    if (context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                    {
                        context.Result = new ContentResult() { Content = "你没有该操作权限", StatusCode = 403 };
                        return;
                    }
                    context.Result = new JsonResult(new AjaxResult { Status = "error", Data = "你没有操作权限" });
                    return;
                }
            }

        }
    }
}
