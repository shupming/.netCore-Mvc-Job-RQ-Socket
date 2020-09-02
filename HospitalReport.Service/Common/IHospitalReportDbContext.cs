
using HospitalReport.SqlSugar;
using System;
namespace HospitalReport.Service.Common
{
   
    public interface IHospitalReportDbContext 
    {

        SqlSugarClient GetDbClient();
    }
}
