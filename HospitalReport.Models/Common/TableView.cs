using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace HospitalReport.Models.Common
{
    public class TableView<T>
    {
        [JsonProperty("list")]
        public IEnumerable<T> DataSource { get; set; }
        [JsonProperty("pagination")]
        public PaginationView Pagination { get; set; }
    }
}
