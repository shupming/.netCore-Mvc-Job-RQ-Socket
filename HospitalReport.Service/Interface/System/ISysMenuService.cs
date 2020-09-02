using HospitalReport.Models.Common;
using HospitalReport.Models.DataBase;
using HospitalReport.Models.ViewModel;
using System.Collections.Generic;

namespace HospitalReport.Service.Interface.System
{
    public interface ISysMenuService
    {
        ReturnedDataResult getList();
    }
}