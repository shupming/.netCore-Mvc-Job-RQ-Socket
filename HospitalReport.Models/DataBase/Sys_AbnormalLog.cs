using HospitalReport.Models.Common;

namespace HospitalReport.Models.DataBase
{
   public class Sys_AbnormalLog : AuditedModel<int>
    {
        public string Body { set; get; }
        public string ClientIp { set; get; }
        public string ErrorMessage { set; get; }
        public string RequestUrl { set; get; }
    }
}
