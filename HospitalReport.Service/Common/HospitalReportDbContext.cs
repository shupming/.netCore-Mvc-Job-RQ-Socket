using HospitalReport.SqlSugar;
using System;

namespace HospitalReport.Service.Common
{
    public class HospitalReportDbContext : IHospitalReportDbContext
    {
        public HospitalReportDbContext(ConnectionConfig connnectionString)
        {
            ConnectionConfig = connnectionString;
        }
        public ConnectionConfig ConnectionConfig { get; }
        public SqlSugarClient GetDbClient()
        {
            SqlSugarClient db = new SqlSugarClient(ConnectionConfig);
            db.Ado.IsEnableLogEvent = true;
            return db;
        }
    }

}
