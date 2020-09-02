using Microsoft.AspNetCore.Mvc;
using HospitalReport.Models.DataBase;
using HospitalReport.Service.Interface.System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Security.Claims;
using HospitalReport.Models.Common;
using HospitalReport.Controllers.BaseCommon;
using HospitalReport.Common;
using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using HospitalReport.Extention;
using System.Threading;

namespace HospitalReport.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ISysUserService _sysUserService;
        private readonly ISysAbnormalLogService _sysAbnormalLogService;
        public int i = 0;

        public HomeController(ISysUserService sysUserService, ISysAbnormalLogService sysAbnormalLogService)
        {
            _sysUserService = sysUserService;
            _sysAbnormalLogService = sysAbnormalLogService;
        }

        public IActionResult LoginOut()
        {
            HttpContext.Response.Cookies.Delete("ShuAdminCookie");
            return Json(new ReturnedDataResult { Data = "操作成功", Status = ReturnedStatus.LoginOut });
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Logindata([FromBody]InputModel<Sys_User> input)
        {
            var returnedData = _sysUserService.LoginVerify(input);
            if (returnedData.Status == ReturnedStatus.Success)
            {
                var model = input.InputView;
                var claims = new[] { new Claim("UserName", model.UserName), new Claim("Password", model.Password) };
                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
                ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);
                Task.Run(async () =>
                {
                        //可以使用HttpContext.SignInAsync方法的重载来定义持久化cookie存储用户认证信息，例如下面的代码就定义了用户登录后60分钟内cookie都会保留在客户端计算机硬盘上，
                        //即便用户关闭了浏览器，60分钟内再次访问站点仍然是处于登录状态，除非调用Logout方法注销登录。
                        //注意其中的AllowRefresh属性，如果AllowRefresh为true，表示如果用户登录后在超过50%的ExpiresUtc时间间隔内又访问了站点，就延长用户的登录时间（其实就是延长cookie在客户端计算机硬盘上的保留时间），
                        //例如本例中我们下面设置了ExpiresUtc属性为60分钟后，那么当用户登录后在大于30分钟且小于60分钟内访问了站点，那么就将用户登录状态再延长到当前时间后的60分钟。但是用户在登录后的30分钟内访问站点是不会延长登录时间的，
                        //因为ASP.NET Core有个硬性要求，是用户在超过50%的ExpiresUtc时间间隔内又访问了站点，才延长用户的登录时间。
                        //如果AllowRefresh为false，表示用户登录后60分钟内不管有没有访问站点，只要60分钟到了，立马就处于非登录状态（不延长cookie在客户端计算机硬盘上的保留时间，60分钟到了客户端计算机就自动删除cookie）
                        await HttpContext.SignInAsync("CookieAuthentication",
                                user, new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                            AllowRefresh = true
                        });
                }).Wait();
                return Json(returnedData);
            }
            return Json(returnedData);
        }
    }
}