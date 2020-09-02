using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
namespace HospitalReport.Models.Common
{
    public class AuditedModel<TPrimaryKey>
    {
        public virtual string CreationUserName { get; set; }
        public virtual string CreationFullName { get; set; }
        public  DateTime CreationTime { get; set; }
        public virtual string LastModificationUserName { get; set; }
        public virtual string LastModificationFullName { get; set; }
        public  DateTime? LastModificationTime { get; set; }
        public TPrimaryKey Id { set; get; }
    }
}
