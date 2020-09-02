using HospitalReport.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.DataBase
{
    public class Sys_User : AuditedModel<int>
    {
        public string UserName { set; get; }
        public string Password { set; get; }
        public string FullName { set; get; }
    }
}