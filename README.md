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
            
            
            
            
            
