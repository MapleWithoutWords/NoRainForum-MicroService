using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NoRainForum.Computer.FrontWeb.Models;
using NoRainForumCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoRainForum.Computer.FrontWeb.Filters
{
    public class NoRainFrontAuthorizatinFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attris = (CheckLoginAttribute[])((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(CheckLoginAttribute), false);
            if (attris.Length<1)
            {
                return;
            }
            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString(ConstList.USERID)))
            {
                if (context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    context.Result = new RedirectResult("/login/index");
                    return;
                }
                context.Result = new JsonResult(new AjaxResult { Status = "redirect", Data = "/login/index" });
                return;
            }
        }
    }
}
