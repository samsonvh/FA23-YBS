using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class DealPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public string? Beginning { get; set; }
        public string? Destination { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public EnumRouteStatus? Status { get; set; }
    }
}
