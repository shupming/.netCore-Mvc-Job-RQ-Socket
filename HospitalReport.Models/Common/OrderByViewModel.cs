using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.Common
{
    public class OrderByViewModel
    {
        public string Field { get; set; }

        public OrderByTypeViewModel Order { get; set; }
        public override string ToString()
        {
            return $" {Field} {Order.ToString()} ";
        }
    }
    public enum OrderByTypeViewModel
    {
        Asc,
        Desc
    }
}
