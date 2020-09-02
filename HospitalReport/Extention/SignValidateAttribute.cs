using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalReport.Extention
{
    public class SignValidateAttribute : ActionFilterAttribute
    {
        #region
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //从文件流中读取传递测参数
            using (var ms = new MemoryStream())
            {
                context.HttpContext.Request.Body.Seek(0, 0);
                context.HttpContext.Request.Body.CopyTo(ms);
              
                var b = ms.ToArray();
                var postParamsString = Encoding.UTF8.GetString(b);
                await next();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //string dataJson = GetContextJson(context.);
            return base.OnResultExecutionAsync(context, next);
        }
        #endregion

    }
}
