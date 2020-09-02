using HospitalReport.Common;
using HospitalReport.Models;
using HospitalReport.Models.Common;
using HospitalReport.Models.DataBase;
using HospitalReport.Models.ViewModel;
using HospitalReport.Service.Common;
using HospitalReport.Service.Interface.System;
using HospitalReport.SqlSugar;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace HospitalReport.Service.Implement.System
{
    public class SysUserService : BaseService<Sys_User>, ISysUserService
    {
     

        public SysUserService(IHospitalReportDbContext dbContext) : base(dbContext)
        {
       
        }

        public List<Sys_User> GetAllList()
        {
            return GetList();
        }

        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="userLoginView">{ "language":"zh_cn","inputView":{"UserName":"admin","Password":"123456"}}</param>
        /// <returns></returns>
        public ReturnedDataResult LoginVerify(InputModel<Sys_User> userLoginView)
        {
            var result = new ReturnedDataResult { Status = ReturnedStatus.Error };
            if (userLoginView?.InputView == null)
            {
                result.Message = "用户名或密码不正确";
                return result;
            }
            var loginInputView = userLoginView.InputView;
            var inputUserName = loginInputView.UserName;
            var inputPassword = loginInputView.Password;
            if (string.IsNullOrEmpty(inputUserName) || string.IsNullOrEmpty(inputPassword))
            {
                result.Message = "用户名或密码不正确";
                return result;
            }
            var userEntity = this.Get(t => t.UserName == inputUserName);

            if (userEntity == null)
            {
                result.Message = "用户名或密码不正确";
                return result;
            }
            if (string.Compare(userEntity.Password, inputPassword) != 0)
            {
                result.Message = "用户名或密码不正确";
                return result;
            }
            //登录用户信息
            var userLoginedViewModel = new Sys_User()
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                FullName = userEntity.FullName,
            };
            result.Message = "登录成功";
            result.Data = userLoginedViewModel;
            result.Status = ReturnedStatus.Success;
            return result;
        }
    }
}