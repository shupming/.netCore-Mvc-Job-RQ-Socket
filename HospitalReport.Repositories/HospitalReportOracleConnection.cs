using System;
using System.Collections.Generic;
using System.Text;
using HospitalReport.SqlSugar;

namespace HospitalReport.Repositories
{
    public class HospitalReportOracleConnection : IHospitalReportConnection
    {
        public HospitalReportOracleConnection(ConnectionConfig connnectionString) {

            ConnectionConfig = connnectionString;
        }
        public ConnectionConfig ConnectionConfig { get; }
        public  SqlSugarClient GetDbClient()
        {
            SqlSugarClient db = new SqlSugarClient(ConnectionConfig);
            db.Ado.IsEnableLogEvent = true;
            return db;
        }
      
    }
}
