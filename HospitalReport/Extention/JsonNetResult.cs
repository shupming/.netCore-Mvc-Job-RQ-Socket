using HospitalReport.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HospitalReport.Extention
{
    public class JsonNetResult : ActionResult
    {
        private readonly ReturnedDataResult _returnedData;

        public JsonNetResult(object data)
        {
            _returnedData = new ReturnedDataResult() { Data = data, Status = ReturnedStatus.Success };
        }

        public JsonNetResult(ReturnedDataResult returnedData)
        {
            this._returnedData = returnedData;
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("序列化内容不能为null");
            }
            if (context.HttpContext == null || context.HttpContext.Response == null)
            {
                return;
            }
            context.HttpContext.Response.ContentType = string.IsNullOrEmpty(context.HttpContext.Response.ContentType) ? "application/json" : context.HttpContext.Response.ContentType;
            if (_returnedData != null)
            {
                context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(value: _returnedData));
            }
        }
    }
}