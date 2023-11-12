using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class ServicePageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public EnumServiceType? Type { get; set; }
        public EnumServiceStatus? Status { get; set; }
    }
}
