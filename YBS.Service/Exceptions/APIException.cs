using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace YBS.Service.Exceptions
{
    public class APIException : Exception
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string MessageContent { get; set; }
        public object Content { get; set; }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public APIException(int statusCode, string errorCode, string message = null, object value = null)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            MessageContent = message;
            Content = value;
        }
    }
}