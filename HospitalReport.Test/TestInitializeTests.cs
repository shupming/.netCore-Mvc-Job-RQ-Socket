using HospitalReport.Service.Common;
using HospitalReport.SqlSugar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HospitalReport.Test
{
    [TestClass]
    public class TestInitializeTests
    {
        [TestInitialize]
        public void TestInitializeTest()
        {
            var services = new ServiceCollection();
 
            var config = new ConnectionConfig
            {
                ConnectionString = "192.168.200.84;Port=3306;Database=v100r001c03_wms",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            };
            services.AddTransient<IHospitalReportDbContext>(s => new HospitalReportDbContext(config));
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
            services.BuildServiceProvider();
        }
    }
}
