using Quartz;
using System.Threading.Tasks;
using HospitalReport.Service.Interface.System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
namespace HospitalReportJob.TimingTaskServices
{
    [DisallowConcurrentExecution]
    public class SynsSysUserJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Log.Information("SynsSysUserJob开始 ...");
            var _userService = RegisterServices.provider.GetRequiredService<ISysUserService>();
         //   var fileData = _userService.GetBoxProduct();
            Log.Information($"SynsSysUserJob结果：");
            return Task.FromResult(true);

        }
    }
}
