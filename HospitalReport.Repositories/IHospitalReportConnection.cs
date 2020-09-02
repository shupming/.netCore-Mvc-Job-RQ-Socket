using HospitalReport.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Repositories
{
    public interface IHospitalReportConnection
    {
        ConnectionConfig ConnectionConfig { get; }
    }
}
