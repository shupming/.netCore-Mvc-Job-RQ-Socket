using HospitalReport.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.ViewModel
{
    public class Bd_Province
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, IsOnlyIgnoreInsert=true)]
        public int Id { set; get; }
        ///summary
        /// 洲/省名称
        ///summary
        public string ProvinceName { get; set; }
        ///summary
        /// 洲/省简称
        ///summary
        public string ProvinceShortName { get; set; }
        ///summary
        /// 所属国家ID
        ///summary
        public int CountryId { get; set; }
        ///summary
        /// 所属国家简称
        ///summary
        public string CountryShortName { get; set; }

        public  int CreatorId { get; set; }
        public  string Creator { get; set; }
        public  DateTime CreationTime { get; set; }


        public  int? LastModifierId { get; set; }
        public  string LastModifier { get; set; }
        public  DateTime? LastModificationTime { get; set; }
    }
}
