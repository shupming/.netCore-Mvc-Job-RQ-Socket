using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalReport.SqlSugar
{
    public class SugarTenant
    {
        public SqlSugarProvider Context { get; set; }
        public ConnectionConfig ConnectionConfig { get; set; }
    }
}
