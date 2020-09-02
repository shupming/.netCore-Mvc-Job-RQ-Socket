using HospitalReport.Models.Common;
using HospitalReport.Models.DataBase;
using HospitalReport.Models.ViewModel;
using HospitalReport.Service.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HospitalReport.Service.Interface.System
{
    public interface ISysUserService : IBaseService<Sys_User>
    {
        List<Sys_User> GetAllList();

        ReturnedDataResult LoginVerify(InputModel<Sys_User> userLoginView);

     //   byte[] GetBoxProduct();
    }
}