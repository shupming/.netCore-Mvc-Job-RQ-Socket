using HospitalReport.SqlSugar;
using System;
namespace HospitalReport.Repositories
{
   
    public interface IHospitalReportDbContext : IDisposable
    {

        SqlSugarClient GetDbClient();
    }
}
