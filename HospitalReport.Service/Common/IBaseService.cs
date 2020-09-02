using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Service.Common
{
   public interface IBaseService<T>: IRepository<T,int>
    {
    }
}
