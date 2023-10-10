using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.PageRequests
{
    public class DefaultPageRequest
    {
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string? OrderBy { get; set; }
        public string? Direction { get; set; }
    }
}