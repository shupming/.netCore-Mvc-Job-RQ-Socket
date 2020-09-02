using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.ViewModel
{
    public class All_Postal_Codes
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { set; get; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { set; get; }
        /// <summary>
        /// 地名
        /// </summary>
        public string PlaceName { set; get; }
        /// <summary>
        /// 州
        /// </summary>
        public string State { set; get; }
       /// <summary>
       /// 州 简称
       /// </summary>
        public string state_code { set; get; }
        /// <summary>
        /// 城市
        /// </summary>
        public string county { set; get; }
        /// <summary>
        ///  社区
        /// </summary>
        public string community { set; get; }
    }
}
