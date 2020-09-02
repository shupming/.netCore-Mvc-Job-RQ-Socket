using HospitalReport.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Repositories
{
    public class HospitalReportDbContext : IHospitalReportDbContext, IDisposable
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
        public void Dispose()
        {

        }
 
    }

}
