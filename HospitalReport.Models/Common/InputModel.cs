using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
namespace HospitalReport.Models.Common
{
    public class InputModel<TInputView> 
    {
      
        public TInputView InputView { get; set; }
        public string Language { get; set; }
    }

    public class ReturnDataList<TInputView>
    {

        public TInputView Data { get; set; }
    }
}
