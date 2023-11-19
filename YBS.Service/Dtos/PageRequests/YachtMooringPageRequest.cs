using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.PageRequests
{
    public class YachtMooringPageRequest : DefaultPageRequest
    {
        public string? YachtName { get; set; }
        public string? DockName { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
    }
}