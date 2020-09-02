using Microsoft.AspNetCore.Mvc;
using HospitalReport.Models.DataBase;
using HospitalReport.Service.Interface.System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Security.Claims;
using HospitalReport.Models.Common;
using Microsoft.AspNetCore.Authorization;
using HospitalReport.Controllers.BaseCommon;
using HospitalReport.Extention;

namespace HospitalReport.Controllers.SysManagement
{
    public class SysMenuController : BaseController
    {
        private readonly ISysUserService _sysUserService;
        private readonly ISysMenuService _sysMenuOperationService;

        public SysMenuController(ISysUserService sysUserService, ISysMenuService sysMenuOperationService)
        {
            _sysUserService = sysUserService;
            _sysMenuOperationService = sysMenuOperationService;
        }

        public IActionResult Login()
        {
            return View();
        }

        //[CheckLoginAttribute]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMenuList([FromBody]InputModel<int> input)
        {
            return Json(_sysMenuOperationService.getList());
        }
    }
}