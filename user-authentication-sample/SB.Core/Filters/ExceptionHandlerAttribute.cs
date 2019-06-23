using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SB.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string message = "Beklenmeyen Bir Hata Oluştu";
            string stack = null;
#if DEBUG
            message = context.Exception.GetBaseException().Message;
            stack = context.Exception.StackTrace;
#endif
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
