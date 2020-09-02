using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalReport.Models.Common
{
    public class PaginationView
    {
        [JsonProperty("current")]
        public int Current { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("pageSizeOptions")]
        public List<string> PageSizeOptions { get; set; } = new List<string>() { "20", "50", "100", "200" };
    }
}
