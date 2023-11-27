using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace YBS.Service.Exceptions
{
    public class SingleAPIException : Exception
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public SingleAPIException(int StatusCode,string Message)
        {
            this.Message = Message;
            this.StatusCode = StatusCode;
        }

    }
}