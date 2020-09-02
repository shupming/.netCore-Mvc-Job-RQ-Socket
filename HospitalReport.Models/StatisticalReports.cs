using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HospitalReport.Models
{
    public class StatisticalReports
    {
        /// <summary>
        /// 导出当前时间
        /// </summary>
        [DisplayName("导出当前时间")]
        public string DateTimeStr { set; get; }
        [DisplayName("客户编号")]
        public string CustomerCode { set; get; }
        [DisplayName("仓库ID")]
        public int WarehouseId { set; get; }
        [DisplayName("海外仓sku")]
        public string ProductSku { set; get; }
        [DisplayName("关联sku (erp sku)")]
        public string RelationSku { set; get; }
        [DisplayName("运输中")]
        public int OnWayQuantity { set; get; }
        [DisplayName("可用库存")]
        public int UsableQuantity { set; get; }
        [DisplayName("当前库存")]
        public int CurrentQuantity { set; get; }
    }
}
