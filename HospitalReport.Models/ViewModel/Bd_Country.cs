using HospitalReport.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.ViewModel
{
    public class Bd_Country
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, IsOnlyIgnoreInsert = true)]
        public int Id { set; get; }

        ///summary
        /// 国家名称
        ///summary
        public string CountryName { get; set; }
        ///summary
        /// 国家英文名称
        ///summary
        public string CountryNameEn { get; set; }
        ///summary
        /// 简称
        ///summary
        public string CountryShortName { get; set; }
        ///summary
        /// 本国语言
        ///summary
        public string NativeLanguage { get; set; }

        public int CreatorId { get; set; }
        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }


        public int? LastModifierId { get; set; }
        public string LastModifier { get; set; }
        public DateTime? LastModificationTime { get; set; }

    }
}
