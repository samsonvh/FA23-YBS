using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Exceptions
{
    public class AggregateAPIException : APIException
    {
        public List<APIException> Exceptions { get; }
        public int StatusCode { get; set; }
        public AggregateAPIException(List<APIException> Exceptions , int StatusCode, string Message) : base(Message)
        {
            this.Exceptions = Exceptions;
            this.StatusCode = StatusCode;
        }
    }
}