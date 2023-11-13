using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class TripPageRequest : DefaultPageRequest
    {
        public EnumTripStatus? Status { get; set; }
    }
}
