

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalReport.SqlSugar
{
    public class DemoBase
    {
        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "Server=192.168.200.84;Port=3306;Database=v100r001c03_oms;Uid=root;Pwd=123456;", DbType = DbType.MySql, IsAutoCloseConnection = true });
            db.Ado.IsEnableLogEvent = true;
   
            return db;
        }
    }
}
