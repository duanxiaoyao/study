using Common.Filters;
using Common.Utils.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理（系统异常，警告,迁移）
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            var message = "";
            if (context.Exception.InnerException is Warning)
            {
                var exception = context.Exception.InnerException as Warning;
                message = exception.Messages;
            }
            else if (context.Exception.InnerException is Transform)
            {
                var exception = context.Exception.InnerException as Transform;
                message = exception.Messages;
            }
            else
            {
                message = context.Exception.Message;
                NLog.LogManager.GetLogger("SystemErrorTraceLog").Error(message);
            }
            if (context.Exception.InnerException is Transform)
            {
                var ex = context.Exception.InnerException as Transform;
                context.Result = new ApiResult(StateCode.Transform, message, ex.Data);
            }
            else
            {
                context.Result = new ApiResult(StateCode.Fail, message);
            }
        }
    }
}
