using HospitalReport.Common;
using HospitalReport.Models.ViewModel;
using HospitalReport.Service.Interface.System;
using Microsoft.Extensions.Configuration;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitaReportService.QuartzJobs
{
    [DisallowConcurrentExecution]
    public class InventoryStatisticsEmail : IJob
    {
        private readonly ISys_User_Service _userService;

        public InventoryStatisticsEmail(ISys_User_Service userService)
        {
            _userService = userService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information("邮箱发送开始...");
            var path = Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            var warehouseinfo = config.GetSection("warehouse").Value;
            var mailboxs = config.GetSection("mailbox");
           var input = warehouseinfo.Split("|").Select((t, i) => new Warehouse_ViewModel
            {
                WarehouseId = Convert.ToInt32(t.Split("-")[0]),
                WarehouseName = t.Split("-")[1],
                CustomerCodes = t.Split("-")[2]
            }).ToList();
            var data = DateTime.UtcNow;
            //"1145192042@qq.com",
            var fileData = _userService.GetProduct(input);
            var title = $"仓库周报统计{DateTime.UtcNow.ToString("yyyy-MM-dd")}";
            string pathFile = $@"{path}\File\{title}.xlsx";
            var mailContent = $"统计时间:UTC时间{data.AddDays(-7).ToString("yyyy-MM-dd hh:mm:ss")} 到 {data.ToString("yyyy-MM-dd hh:mm:ss")} ";
            File.WriteAllBytes(pathFile, fileData);
            var mailTo = mailboxs.Value.Split(",");
            SendEmail.SendtoEmail(mailContent, title, mailboxs.Value.Split(","), pathFile);
        }
    }

}
