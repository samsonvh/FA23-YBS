using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.PageResponses
{
    public class CommonErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}