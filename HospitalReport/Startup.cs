using HospitalReport.Common;
using HospitalReport.Service.Common;
using HospitalReport.SqlSugar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq;
using System;
using HospitalReport.Models.Common;
using Microsoft.AspNetCore.Mvc;
using AspectCore.Extensions.DependencyInjection;
using AspectCore.Configuration;
using AspectCore.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace HospitalReport
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", c =>
            {
                c.Cookie.Name = "ShuAdminCookie";
                c.LoginPath = "/Home/Logindata";
            });
            services.AddSession(options =>
            {
                var sessionTimeout = Convert.ToInt32(Configuration.GetSection("SessionTimeout")?.Value ?? "30");
                // 设置 Session 过期时间15分钟
                options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
            });
            var config = new ConnectionConfig
            {
                ConnectionString = Configuration.GetSection("ConnectionString").Value,
                DbType = (DbType)Enum.Parse(typeof(DbType), Configuration.GetSection("DbType").Value),
                IsAutoCloseConnection = true
            };
            services.AddTransient<IHospitalReportDbContext>(s => new HospitalReportDbContext(config));
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var assemblys = Assembly.Load("HospitalReport.Service").GetTypes();
            var interfaceAssemblys = assemblys.Where(t => t.FullName.StartsWith("HospitalReport.Service.Interface")).ToList();
            var implementAssemblys = assemblys.Where(t => t.FullName.StartsWith("HospitalReport.Service.Implement")).ToList();
            foreach (var item in implementAssemblys)
            {
                var interfaceType = interfaceAssemblys.FirstOrDefault(t => t.FullName.EndsWith($"I{item.Name}"));
                if (interfaceType != null)
                    services.AddTransient(interfaceType, item);
            }
            services.AddMvc().AddJsonOptions(json =>
            {
                json.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                json.JsonSerializerOptions.Converters.Add(new IntToStringConverter());
                json.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                json.JsonSerializerOptions.IgnoreNullValues = true;
                json.JsonSerializerOptions.PropertyNamingPolicy = null;
                //允许额外符号
                json.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                json.JsonSerializerOptions.PropertyNameCaseInsensitive = false;

            });
            //根据属性注入来配置全局拦截器
            services.ConfigureDynamicProxy(config1 =>
            {
                config1.Interceptors.AddTyped<ErrorTryCatchAttribute>();//Attribute.ErrorTryCatchAttribute这个是需要全局拦截的拦截器
            });  
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddControllersAsServices();
          

            //Scoped  1
            //指定将为每个作用域创建服务的新实例。
            //Singleton   0
            //指定将创建该服务的单个实例。
            //Transient   2
            //指定每次请求服务时，将创建该服务的新实例。
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            FrameWorkService.ServiceProvider = app.ApplicationServices;
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(fileServerOptions);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();//Authentication中间件，该中间件会根据当前Http请求中的Cookie信息来设置HttpContext.User属性
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                         name: "default",
                         pattern: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}