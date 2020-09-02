# .netCore-Mvc-Job-RQ-Socket
用.NetCore 搭建的MVC , Job服务(QZ) 消息队列RQ ,Socket 前后端分离   前端Vue-element    

            services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", c =>
            {
                c.Cookie.Name = "ShuAdminCookie";
                c.LoginPath = "/Home/Logindata";
            }); //设置 Cookie 名称
            services.AddSession(options =>
            {
                var sessionTimeout = Convert.ToInt32(Configuration.GetSection("SessionTimeout")?.Value ?? "30");
                // 设置 Session 过期时间15分钟
                options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
            });//配置 Session时间 
            var config = new ConnectionConfig
            {
                ConnectionString = Configuration.GetSection("ConnectionString").Value,
                DbType = (DbType)Enum.Parse(typeof(DbType), Configuration.GetSection("DbType").Value),
                IsAutoCloseConnection = true
            };
            //依赖注册
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
            //text.json 格式配置
            services.AddMvc().AddJsonOptions(json =>
            {// 时间格式配置
                json.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                // string 转int 配置
                json.JsonSerializerOptions.Converters.Add(new IntToStringConverter());
                //编码配置
                json.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                // 不反会值为空的字段
                json.JsonSerializerOptions.IgnoreNullValues = true;
                json.JsonSerializerOptions.PropertyNamingPolicy = null;
                //允许额外符号
                json.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                json.JsonSerializerOptions.PropertyNameCaseInsensitive = false;

            });
            //根据属性注入来配置全局拦截器 （主要解决service 层代码报错，保存入参，异常方法日志）
            services.ConfigureDynamicProxy(config1 =>
            {
                config1.Interceptors.AddTyped<ErrorTryCatchAttribute>();//Attribute.ErrorTryCatchAttribute这个是需要全局拦截的拦截器
            });  
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddControllersAsServices();
            
            //全局保存 依赖注册 后面方便使用
             FrameWorkService.ServiceProvider = app.ApplicationServices;
             
             //前后端分离  项目默认打开 index.html 路由全在全端配置
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            
            
            Job 用.net core 搭建 
            var log = $"{AppDomain.CurrentDomain.BaseDirectory}\\LogFiles\\log.txt"; 日志
            Log.Logger = new LoggerConfiguration().WriteTo.File(log).CreateLogger();
            RegisterServices.RegisterServicesFun(); 依赖注册  注意 一定要服前
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
                x.SetDescription("Job 数据处理，业务逻辑处理 定时任务");
                x.SetDisplayName("HospitalReportJob");
                x.SetServiceName("HospitalReportJob");
                x.EnablePauseAndContinue();
            });
            
            
     public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        //调度器
        private readonly IScheduler scheduler;
        public ServiceRunner()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
        }
        //开始
        public bool Start(HostControl hostControl)
        {
            Log.Information("服务器开始启动");
            scheduler.Start();
            return true;
        }
        //停止
        public bool Stop(HostControl hostControl)
        {
            scheduler.Shutdown(false);
            return true;
        }
        //恢复所有
        public bool Continue(HostControl hostControl)
        {
            scheduler.ResumeAll();
            return true;
        }
        //暂停所有
        public bool Pause(HostControl hostControl)
        {
            scheduler.PauseAll();
            return true;
        }
    }
            
            
            
      前端 vue 
      package.json  配置打包命令
         "build:prod": "vue-cli-service build", 开发环境
         "build": "vue-cli-service build --mode staging", 生成环境
          dev": "vue-cli-service serve", 本地运行
      配置通过不同打包方式 ，调用不接口数据
      .env.staging  .env.production  .env.development 
      在这里可以配置 三个打包配置匿名路径  VUE_CLI_BABEL_TRANSPILE_MODULES 是否调本地store 数据
      
      
      request.js  全系统封装入参，和反参 
       config.data = {
                InputView:config.data,
                Language:store.getters.language
               };
       return config;
     // 反参
    response => {
    const res = response.data
    if (res.Status != 'Success') {
      Message({
        message: res.Message || 'Error',
        type: 'error',
        duration: 5 * 1000
      })
      if (res.Status === "LoginOut" ) {
        location.reload() //退出请求token  页面加载会判断有没有token 没有token 会条登录路由
      }
      return Promise.reject(new Error(res.message || 'Error'))
      
      
      permission.js
      页面加载执行的js  有token 判断，如果有token 就调用菜单接口 加载路由
      
         try {
        await store.dispatch('sysmenus/getmenulist');
        let meuns=store.getters.meuns;
        if (meuns.length < 1) {
              global.antRouter = []
              next()
       }
        router.addRoutes(meuns) // 2.动态添加路由
        global.antRouter = meuns // 3.将路由数据传递给全局变量，做侧边栏菜单渲染工作
        next({ ...to, replace: true })
        
        退出记得要清空动态路由，要不然登录进去就是空白页面（想了好久 才想到这个方法）
      
      
   消息队列，一键生成菜单和操作权限，sokcet（core） 服务 还在搭建中
