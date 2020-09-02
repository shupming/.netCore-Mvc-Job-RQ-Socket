using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalReport.Extention
{
    public class DefaultModelBinder : IModelBinder
    {
        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            //根据名称获取传递的值
            ValueProviderResult ValueResult = bindingContext.ValueProvider.GetValue(modelName);
            //从请求的参数集合中，拿到第一个参数
            string value = ValueResult.FirstValue;
            bindingContext.Result = ModelBindingResult.Success(value);
            return Task.CompletedTask;
        }
    }

}
