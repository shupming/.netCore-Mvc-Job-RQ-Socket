using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.DataBase
{
    public class Bd_City
    {
        ///summary
        /// 城市名称
        ///summary
        public string CityName { get; set; }
        ///summary
        /// 城市简称
        ///summary
        public string CityShortName { get; set; }
        ///summary
        /// 国家ID
        ///summary
        public int CountryId { get; set; }
        ///summary
        /// 国家简称
        ///summary
        public string CountryShortName { get; set; }
        ///summary
        /// 洲/省ID
        ///summary
        public int ProvinceId { get; set; }
        ///summary
        /// 洲/省简称
        ///summary
        public string ProvinceShortName { get; set; }
    }
}
