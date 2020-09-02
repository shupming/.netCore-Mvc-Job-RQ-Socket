using HospitalReport.Models.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using HospitalReport.Models.DataBase;
using HospitalReport.Service.Interface.System;
using HospitalReport.Common;
using System.IO;

namespace HospitalReport.Extention
{
    public class CheckLoginAttribute : ActionFilterAttribute,IExceptionFilter, IActionFilter
    {
        /// <summary>
        /// 是否忽略验证
        /// </summary>
        private readonly bool _ignore;

        /// <summary>
        /// 构造方法，默认需要验证
        /// </summary>
        /// <param name="ignore"></param>
        public CheckLoginAttribute(bool ignore = false)
        {
            this._ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;
            if (_ignore)
                return;
            if (filterContext.HttpContext.User.Identity.IsAuthenticated )
            {
                var userName = filterContext.HttpContext.User.Claims.First().Value;
                filterContext.HttpContext.Request.Cookies.TryGetValue("ShuAdminCookie", out string value);
                return;
            }
            else
            {
                var returnedData = new ReturnedDataResult()
                {
                    Status = ReturnedStatus.LoginOut,
                    Message = "对不起，您没有登录或登录已超时！"
                };
                filterContext.Result = new JsonNetResult(returnedData);
            }
        }
        /// <summary>
        /// 身份验证过滤
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
        /// <summary>
        /// 异常返回 控制器异常
        /// </summary>
        /// <param name="actionContext"></param>
        public void OnException(ExceptionContext context)
        {
            var id = 0;
            if (context.Exception != null)
            {
                var logService = FrameWorkService.ServiceProvider.GetService<ISysAbnormalLogService>();
                var log = new Sys_AbnormalLog();
                log.Body = "";
                log.ErrorMessage = context.Exception.ToString();
                log.RequestUrl = context.HttpContext.Request.Path;
                log.ClientIp = string.Empty;
                log.SetUserToModelCreate();
                id = logService.InsertReturnIdentity(log);
            }
            var returnedData = new ReturnedDataResult()
            {
                Status = ReturnedStatus.Error,
                Message = $"请联系开发人员 logid:{id}！"
            };
            context.Result = new JsonResult(returnedData);

        }
    }
}