using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalReport.SqlSugar
{
    public class DbResult<T>
    {
        public bool IsSuccess { get; set; }
        public Exception ErrorException { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
