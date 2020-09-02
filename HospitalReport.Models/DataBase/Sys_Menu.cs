using HospitalReport.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.DataBase
{
    public class Sys_Menu : AuditedModel<int>
    {
        public int ParentId { set; get; }
        public string MenuName { set; get; }
        public string MenuNameEn { set; get; }
        public int SequenceId { set; get; }
        public string Icon { set; get; }
        public string Path { set; get; }
        public string PageUrl { set; get; }
        public byte Status { set; get; }

        public string ControllerUrl { set; get; }
        public byte IsSysGenerated { set; get; }
    }
}