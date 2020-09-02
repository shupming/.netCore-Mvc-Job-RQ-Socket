using HospitalReport.Models.Common;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HospitalReport.Common
{
   public class HttpResponse
    {
        public  static string PostJson(string url, string postData, Dictionary<string, string> headers = null, int timeout = 60, string contentType = "application/json")
        {
            if (url.IndexOf("https") > -1)
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            using (HttpClient client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
                client.Timeout = new TimeSpan(0, 0, timeout);
                using (HttpContent content = new StringContent(postData ?? "", Encoding.UTF8))
                {
                    if (contentType != null)
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    }
                    using (HttpResponseMessage responseMessage = client.PostAsync(url, content).Result)
                    {
                        byte[] resultBytes = responseMessage.Content.ReadAsByteArrayAsync().Result;
                        return Encoding.UTF8.GetString(resultBytes);
                    }
                }
            }
        }


        public static ReturnedDataResult<T> PostDataJson<T>(string url, string postData, Dictionary<string, string> headers = null, int timeout = 60,
            string contentType = "application/json")
        {
            try
            {
                var data = PostJson(url, postData, headers, timeout, contentType);
                return new ReturnedDataResult<T>
                {
                    Status = ReturnedStatus.Error,
                    Data = JsonSerializer.Deserialize<T>(data),

                };
            }
            catch (Exception e)
            {

                return new ReturnedDataResult<T>
                {
                    Status = ReturnedStatus.Error,
                    Message = e.Message
                };


            }

        }

    }
}
