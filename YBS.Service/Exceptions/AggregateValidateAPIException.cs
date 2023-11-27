using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Exceptions
{
    public class AggregateValidateAPIException : APIException
    {
        public List<ValidateAPIException> Exceptions { get; }
        public int StatusCode { get; }
        public AggregateValidateAPIException(List<ValidateAPIException> Exceptions, int StatusCode, string Message)
        : base(Message)
        {
            this.Exceptions = Exceptions;
            this.StatusCode = StatusCode;
        }
    }
}