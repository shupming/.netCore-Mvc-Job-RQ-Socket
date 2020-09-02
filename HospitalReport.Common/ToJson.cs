using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
namespace HospitalReport.Common
{
    public static class ToJson
    {
        public static string ObjectToJson(this object data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return JsonSerializer.Serialize(data,
                    options: new System.Text.Json.JsonSerializerOptions
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });
        }
    }
}
