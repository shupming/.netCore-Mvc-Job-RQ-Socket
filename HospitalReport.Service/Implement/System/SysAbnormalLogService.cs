using HospitalReport.Models.DataBase;
using HospitalReport.Service.Common;
using HospitalReport.Service.Interface.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Service.Implement.System
{
    public class SysAbnormalLogService : BaseService<Sys_AbnormalLog>,ISysAbnormalLogService
    {
        public SysAbnormalLogService(IHospitalReportDbContext dbContext) : base(dbContext)
        {

        }
    }
}
