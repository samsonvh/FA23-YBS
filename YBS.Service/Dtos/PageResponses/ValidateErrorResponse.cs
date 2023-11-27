using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.PageResponses
{
    public class ValidateErrorResponse
    {
        public int StatusCode { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
    }
}