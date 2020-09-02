using System;
using HospitalReport.Models.Common;
using System.Text;

namespace HospitalReport.Models.DataBase
{
  public  class Sys_SyncDataRecordHistory : AuditedModel<int>
    {
        ///summary
        /// API地址
        ///summary
        public string ApiUrl { get; set; }
        ///summary
        /// 同步项编码（Job的方法名）
        ///summary
        public string SyncItemCode { get; set; }
        ///summary
        /// 同步项名称
        ///summary
        public string SyncItemName { get; set; }
        ///summary
        /// 最小时间（大于等于）
        ///summary
        public DateTime MinTime { get; set; }
        ///summary
        /// 最大时间（小于）
        ///summary
        public DateTime MaxTime { get; set; }
        ///summary
        /// 时间间隔（单位：分钟）
        ///summary
        public int TimeInterval { get; set; }
        ///summary
        /// 执行总记录数
        ///summary
        public int ExcuteTotalNumber { get; set; }
        ///summary
        /// 执行成功记录数
        ///summary
        public int ExcuteSucessNumber { get; set; }
        ///summary
        /// 执行失败记录
        ///summary
        public int ExcuteFailNumber { get; set; }
        ///summary
        /// 是否分页：0-否 1-是
        ///summary
        public byte IsPaging { get; set; }
        ///summary
        /// 每页数量
        ///summary
        public int PageSize { get; set; }
        ///summary
        /// 同步开始时间
        ///summary
        public DateTime ExcuteStartTime { get; set; }

        public static explicit operator Sys_SyncDataRecordHistory(Sys_SyncDataRecord v)
        {
            throw new NotImplementedException();
        }

        ///summary
        /// 同步完成时间
        ///summary
        public DateTime ExcuteEndTime { get; set; }
        ///summary
        /// 同步总耗时（单位：秒）
        ///summary
        public int ExcuteDuration { get; set; }
        ///summary
        /// 状态：1-停用 2-启用
        ///summary
        public byte Status { get; set; }
        ///summary
        /// 异常信息
        ///summary
        public string ErrorMessage { get; set; }
        ///summary
        /// 执行状态 1-未执行 2-已执行完 3-执行失败
        ///summary
        public byte ExcuteStatus { get; set; }
        ///summary
        /// 分布式服务器Id(配置在winservice中的DistributedServerId节点信息)
        ///summary
        public int? DistributedServerId { get; set; }
    }
}
