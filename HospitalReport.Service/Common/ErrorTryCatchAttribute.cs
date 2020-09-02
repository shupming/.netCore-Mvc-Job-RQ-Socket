using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using HospitalReport.Common;
using HospitalReport.Models.Common;
using HospitalReport.Models.DataBase;
using HospitalReport.Service.Interface.System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalReport.Service.Common
{
    public class ErrorTryCatchAttribute : AbstractInterceptorAttribute
    {
        [HandleProcessCorruptedStateExceptions]
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                await context.Invoke(next);
            }
            catch (Exception ex)
            {
                if (!ex.Message.EndsWith($"-{ReturnedStatus.AbnormalCode}."))
                {
                    var logService = FrameWorkService.ServiceProvider.GetService<ISysAbnormalLogService>();
                    var log = new Sys_AbnormalLog();
                    log.Body = context.Parameters.ObjectToJson();
                    log.ErrorMessage = ex.ToString();
                    log.RequestUrl = context.ImplementationMethod.ReflectedType.FullName + "." + context.ImplementationMethod.Name;
                    log.ClientIp = FrameWorkService.ServiceProvider.GetService<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.ToString();
                    log.SetUserToModelCreate();
                    var id = logService.InsertReturnIdentity(log);
                    var returnedData = new ReturnedDataResult()
                    {
                        Status = ReturnedStatus.AbnormalCode,
                        Message = $"请联系开发人员 logid:{id}！"
                    };
                    ////创建并返回继承自ReturnedResult的return对象
                    if (context.ProxyMethod.ReturnType == typeof(ReturnedDataResult))
                    {
                        context.ReturnValue = returnedData;
                    }
                    else
                    {
                        throw new Exception($"@{returnedData.Message}-{ returnedData.Status }");
                    }
                }
                else {
                    if (context.ProxyMethod.ReturnType == typeof(ReturnedDataResult))
                    {
                        context.ReturnValue = new ReturnedDataResult()
                        {
                            Status = ReturnedStatus.AbnormalCode,
                            Message = ex.Message.Substring(ex.Message.IndexOf("@")+1).Replace($"-{ReturnedStatus.AbnormalCode}.",string.Empty)
                        };
                    }
                    else {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}
