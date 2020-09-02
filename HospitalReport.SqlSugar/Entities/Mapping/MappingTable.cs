using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalReport.SqlSugar
{
    public class MappingTable
    {
        public string EntityName { get; set; }
        public string DbTableName { get; set; }
        public string DbShortTaleName { get; set; }
    }
}
