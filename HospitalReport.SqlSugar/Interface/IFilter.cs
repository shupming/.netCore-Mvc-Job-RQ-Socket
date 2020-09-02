﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalReport.SqlSugar
{
    public interface IFilter
    {
        IFilter Add(SqlFilterItem filter);
        void Remove(string filterName);
        List<SqlFilterItem> GeFilterList { get; }
    }
}
