using HospitalReport.Models.DataBase;
using HospitalReport.Repositories.Interface.System;
using System.Collections.Generic;

namespace HospitalReport.Repositories.Implement.System
{
    public class Sys_User_Repository : Repository<Sys_User, int>, ISys_User_Repository
    {
        public Sys_User_Repository(IHospitalReportDbContext hospitalReportDbContext) : base(hospitalReportDbContext) { 
        
        }
     
    }
}
