using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainForumCommon
{
    public class NoRainActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    context.Result = new ContentResult() { Content = ModelStateValid.GetErrorMsg(context.ModelState) };
                    return;
                }
                else
                {
                    context.Result = new JsonResult(new AjaxResult { Status = "error", ErrorMsg = ModelStateValid.GetErrorMsg(context.ModelState) });
                    return;
                }
            }
        }
    }
}
