using LinqToDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Serilog;
using System.Threading.Tasks;
using System.IO;
using HospitalReport.Service.Common;
using System.Reflection;
using System.Linq;
using HospitalReport.SqlSugar;
using System;
using HospitaReportService.QuartzJobs;
using Topshelf;

namespace HospitaReportService
{
    public class SyncService
    {
    
        public async Task StartAsync()
        {
            var provider = RegisterServices();
            Scheduler = provider.GetService(typeof(IScheduler)) as IScheduler;
            await Scheduler.Start();
            Log.Information("Quartz调度已启动...");
        }

        public async Task StopAsync()
        {
            await Scheduler.Shutdown();
            Log.Information("Quartz调度结束...");
            Log.CloseAndFlush();
        }
        public bool Continue(HostControl hostControl)
        {
            Scheduler.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            Scheduler.PauseAll();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Scheduler.Shutdown();
            Console.WriteLine("Quartz任务调度关闭");
            return true;
        }
        #region Utils
        private IScheduler Scheduler { get; set; }
        private static ServiceProvider RegisterServices()
        {
            Log.Information("配置依赖注入...");
            var configuration = ReadFromAppSettings();
            var services = new ServiceCollection();
            services.AddScoped<SyncService>();
            #endregion

            #region Quartz

            Log.Information("配置Quartz...");

            var config = new ConnectionConfig
            {
                ConnectionString = configuration.GetSection("ConnectionString").Value,
                DbType = (DbType)Enum.Parse(typeof(DbType), configuration.GetSection("DbType").Value),
                IsAutoCloseConnection = true
            };
            services.AddScoped<InventoryStatisticsEmail>();
            services.AddScoped<IJobFactory, JobFactory>();
            services.AddScoped<IHospitalReportDbContext>(s => new HospitalReportDbContext(config));
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            var assemblys = Assembly.Load("HospitalReport.Service").GetTypes();
            var interfaceAssemblys = assemblys.Where(t => t.FullName.StartsWith("HospitalReport.Service.Interface")).ToList();
            var implementAssemblys = assemblys.Where(t => t.FullName.StartsWith("HospitalReport.Service.Implement")).ToList();
            foreach (var item in implementAssemblys)
            {
                var interfaceType = interfaceAssemblys.FirstOrDefault(t => t.FullName.EndsWith($"I{item.Name}"));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, item);
            }
            services.AddSingleton(service =>
            {
                var option = new QuartzOption(configuration);
                var sf = new StdSchedulerFactory(option.ToProperties());
                var scheduler = sf.GetScheduler()?.Result  ;
                scheduler.JobFactory = service.GetService<IJobFactory>();
                return scheduler;
            });
            #endregion

            var provider = services.BuildServiceProvider();
            return provider;
        }

        private static IConfigurationRoot ReadFromAppSettings()
        {
            //读取appsettings.json
           return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
