using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.Common
{
   
    public class PropertyModel : PropertyNameModel
    {
        public object Value { get; set; }

        public TypeCode TypeCode { get; set; }
    }
}
