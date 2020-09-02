using HospitalReport.Service.Common;
using HospitalReport.SqlSugar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Reflection;

namespace HospitalReportJob
{
    public class RegisterServices
    {
        public static ServiceProvider RegisterServicesFun()
        {
            Log.Information("配置依赖注入...");
            var configuration = ReadFromAppSettings();
            var services = new ServiceCollection();
            #region Quartz

            Log.Information("配置Quartz...");

            var config = new ConnectionConfig
            {
                ConnectionString = configuration.GetSection("ConnectionString").Value,
                DbType = (DbType)Enum.Parse(typeof(DbType), configuration.GetSection("DbType").Value),
                IsAutoCloseConnection = true
            };
            services.AddSingleton<IHospitalReportDbContext>(s => new HospitalReportDbContext(config));
            services.AddSingleton(typeof(IRepository<,>), typeof(Repository<,>));
            var assemblys = Assembly.Load("HospitalReport.Service").GetTypes();
            var interfaceAssemblys = assemblys.Where(t => t.FullName.StartsWith("HospitalReport.Service.Interface")).ToList();
            var implementAssemblys = assemblys.Where(t => t.FullName.StartsWith("HospitalReport.Service.Implement")).ToList();
            foreach (var item in implementAssemblys)
            {
                var interfaceType = interfaceAssemblys.FirstOrDefault(t => t.FullName.EndsWith($"I{item.Name}"));
                if (interfaceType != null)
                    services.AddSingleton(interfaceType, item);
            }
            #endregion
            provider = services.BuildServiceProvider();
            return provider;
        }
        public static ServiceProvider provider { set; get; }
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
