using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Admin.DTO;
using Services.Admin.IService;
using Services.APICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Admin.API.Filters
{
    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        public IPermissionService PerSvc { get; set; }
        public AdminAuthorizationFilter(IPermissionService PerSvc)
        {
            this.PerSvc = PerSvc;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            LoginAttribute[] pers = (LoginAttribute[])
                ((ControllerActionDescriptor)context.ActionDescriptor)
                .MethodInfo.GetCustomAttributes(typeof(LoginAttribute), false);
            if (pers.Length < 1)
            {
                return;
            }

            string token = JWTHelper.GetToken(context.HttpContext, "token");
            if (!JWTHelper.Decrypt(token, out ListAdminUserDTO adminUser))
            {
                context.Result = new JsonResult(new APIResult<long> { ErrorMsg = "请先登录！" }) { StatusCode = 401 };
                return;
            }
            foreach (var attr in pers)
            {
                var task = PerSvc.CheckPermissionAsync(adminUser.Id, attr.PermissionName);
                task.Wait();
                if (!task.Result)
                {

                    context.Result = new JsonResult(new APIResult<long> { ErrorMsg = $"您没有该权限！" }) { StatusCode = 401 };
                    return;
                }
            }

        }
    }
}
