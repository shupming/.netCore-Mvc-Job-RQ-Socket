using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.DataBase
{
    public class Lg_AccountInfo
    {
        public int Id { get; set; }

        ///summary
        /// GUID
        ///summary
        public string AccountGuid { get; set; }

        ///summary
        /// 用户名
        ///summary
        public string UserName { get; set; }

        ///summary
        /// 密码
        ///summary
        public string Password { get; set; }

        ///summary
        /// 账户AccessLicenseNumber
        ///summary
        public string AccessLicenseNumber { get; set; }

        ///summary
        /// Host
        ///summary
        public string Host { get; set; }

        ///summary
        /// 备注json
        ///summary
        public string Remarks { get; set; }

        public int? LastModifierId { get; set; }
        public string LastModifier { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public int CreatorId { get; set; }
        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }
    }
}