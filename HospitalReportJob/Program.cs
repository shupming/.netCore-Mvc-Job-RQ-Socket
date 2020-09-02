using Serilog;
using System;
using Topshelf;

namespace HospitalReportJob
{
    public class Program
    {
        /// <summary>
        /// 主程序入口
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var log = $"{AppDomain.CurrentDomain.BaseDirectory}\\LogFiles\\log.txt";
            Log.Logger = new LoggerConfiguration().WriteTo.File(log).CreateLogger();
            RegisterServices.RegisterServicesFun();
            HostFactory.Run(x =>
            {
                x.Service<ServiceRunner>(s => {
                    s.ConstructUsing(name => new ServiceRunner());
                    s.WhenStarted((tc, hc) => tc.Start(hc));
                    s.WhenStopped((tc, hc) => tc.Stop(hc));
                    s.WhenContinued((tc, hc) => tc.Continue(hc));
                    s.WhenPaused((tc, hc) => tc.Pause(hc));
                });
                x.RunAsLocalService();
                x.StartAutomaticallyDelayed();
                x.SetDescription("用于系统定时任务计划执行");
                x.SetDisplayName("JobService2");
                x.SetServiceName("JobService2");
                x.EnablePauseAndContinue();
            });
        }
    }
}
