using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.Common
{
    public class QueryViewModel<TInputView> : InputModel<TInputView>
    {
        public QueryViewModel()
        {
            Current = 1;
            PageSize = 20;
            OrderBy = new OrderByViewModel
            {
                Field = "Id",
                Order = OrderByTypeViewModel.Desc
            };
        }
        public int Current { get; set; }
        public int PageSize { get; set; }
        public OrderByViewModel OrderBy { get; set; }
    }

    public class ReturnPageDataModel<TInputView> : ReturnDataList<TInputView>
    {
        public int Current { get; set; }
        public int PageSize { get; set; }
    }
}
