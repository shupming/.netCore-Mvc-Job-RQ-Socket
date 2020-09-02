namespace HospitalReport.Models.Common
{
    public class ReturnedDataResult : ReturnedResul
    {
        public object Data { get; set; }
    }

    public class ReturnedResul
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class ReturnedDataResult<T> : ReturnedResul
    {
        public T Data { get; set; }
    }
}