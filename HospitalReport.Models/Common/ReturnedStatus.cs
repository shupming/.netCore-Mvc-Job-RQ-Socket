using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.Common
{
    public struct ReturnedStatus
    {
        public static string Success = "Success";
        public static string Warning = "Warning";
        public static string LoginOut = "LoginOut";
        public static string Error = "Error";
        public static string AbnormalCode = "AbnormalCode";
    }
}