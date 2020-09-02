
using System;
using System.IO;
using System.Net;

namespace HospitalReport.Common
{
    public static class FileDownFunc
    {
        public static byte[] NewDownloadFile(string fileUrl)
        {
            using (var webClient = new WebClient())
            {
                var bytes = webClient.DownloadData(fileUrl);
                return new MemoryStream(bytes).ToArray();
            }
        }
        /// <summary>
        /// 下载网络图片到流
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static MemoryStream DownloadFile(string fileUrl)
        {
            HttpWebResponse httpWebResponse = null;
            try
            {
                var httpWebRequest = (HttpWebRequest)(WebRequest.Create(fileUrl));
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = true;
                httpWebResponse = (HttpWebResponse)(httpWebRequest.GetResponse());
                MemoryStream ms;
                using (var stream = httpWebResponse.GetResponseStream())
                {
                    var buffer = new byte[httpWebResponse.ContentLength];
                    int offset = 0, actuallyRead;
                    do
                    {
                        actuallyRead = stream.Read(buffer, offset, buffer.Length - offset);
                        offset += actuallyRead;
                    }
                    while (actuallyRead > 0);
                    ms = new MemoryStream(buffer);
                }
                return ms;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }
        }

    }
}
