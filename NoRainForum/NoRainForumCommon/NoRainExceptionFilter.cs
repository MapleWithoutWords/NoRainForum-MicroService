using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainForumCommon
{
    public class NoRainExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<NoRainExceptionFilter> _logger;

        public NoRainExceptionFilter(ILogger<NoRainExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "server exception");
            context.Result = new RedirectResult("/Error/Error500");
        }
    }
}
