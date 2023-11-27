using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace YBS.Service.Exceptions
{
    public class ValidateAPIException : Exception
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public ValidateAPIException(string Message, string Key)
        {
            this.Message = Message;
            this.Key = Key;
        }

    }
}