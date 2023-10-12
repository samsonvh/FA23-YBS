using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class YachtPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public int? MaximumGuestLimit { get; set; }
        public EnumYachtStatus? Status { get; set; }
    }
}
