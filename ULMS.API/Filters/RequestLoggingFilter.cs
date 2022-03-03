using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using ULMS.Helpers;

namespace ULMS.Filters
{
    public class RequestLoggingFilter : IAsyncResultFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger;

        public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
        {
            _logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var result = context.Result as IStatusCodeActionResult;
            var statusCode = HttpStatusCode.OK;

            if (result?.StatusCode != null)
            {
                statusCode = (HttpStatusCode)result.StatusCode;
            }
            _logger.LogInformation(HttpHelper.RequestToLogStringWithHttpStatusCode(context.HttpContext.Request, statusCode));
            await next();
        }
    }
}
