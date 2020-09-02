using HospitalReport.Models.DataBase;


namespace HospitalReport.Service.Common
{
    public class BaseService<T> : Repository<T, int>, IBaseService<T> where T : class, new()
    {
        public BaseService(IHospitalReportDbContext hospitalReportDbContext) : base(hospitalReportDbContext)
        {

        }
    }
}
