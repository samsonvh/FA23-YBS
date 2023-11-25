using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Exceptions
{
    public class AggregateAPIException : Exception
    {
        public List<APIException> Exceptions { get; }
        public int StatusCode { get; set; }
        public AggregateAPIException(List<APIException> exceptions)
        {
            Exceptions = exceptions;
        }
    }
}