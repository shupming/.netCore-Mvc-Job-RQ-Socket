
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AspectCore.Extensions.Hosting;

namespace HospitalReport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            // 这里添加配置文件
            //.AddJsonFile("appsettings.json", true)
            //.Build();
            //WebHost.CreateDefaultBuilder(args)
            //      .UseStartup<Startup>()
            //  .ConfigureAppConfiguration(builder =>
            //  {
            //      builder.AddJsonFile("appsettings.json");
            //  }).UseDynamicProxy().UseConfiguration(config).Build().Run();

           // .UseDynamicProxy()

            Host.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration(builder =>
              {
                  builder.AddJsonFile("appsettings.json").SetBasePath(Directory.GetCurrentDirectory());
                  
              }).ConfigureWebHostDefaults(webBuilder=> {
                  webBuilder.UseStartup<Startup>();
              }).UseDynamicProxy().Build().Run();



        }


    }
}
