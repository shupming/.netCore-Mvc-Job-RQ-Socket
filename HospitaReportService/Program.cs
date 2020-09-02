using Serilog;
using Serilog.Events;
using System;
using Topshelf;

namespace HospitaReportService
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceLog();
            var rc = HostFactory.Run(x =>
            {
                x.Service<SyncService>(s =>
                {
                    s.ConstructUsing(name => new SyncService());
                    s.WhenStarted(async tc => await tc.StartAsync()); //调用此方法前勿有太多操作，会造成服务启动失败
                    s.WhenStopped((tc, hc) => tc.Stop(hc));
                    s.WhenContinued((tc, hc) => tc.Continue(hc));
                    s.WhenPaused((tc, hc) => tc.Pause(hc));
                    s.WhenStopped(async tc => await tc.StopAsync());
                });
                x.RunAsLocalSystem();
                x.SetDescription("SyncJobDescription");
                x.SetDisplayName("SyncJobDisplayName");
                x.SetServiceName("SyncJobServiceName");
            });
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
        private static void InstanceLog()
        {
            //配置Serilog
            var template = "{Timestamp:HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "logs/log.txt", outputTemplate: template, rollingInterval: RollingInterval.Day)
                .WriteTo.Console(LogEventLevel.Information)
                .CreateLogger();
            Log.Information("asdasd");
        }
     
    }
}
