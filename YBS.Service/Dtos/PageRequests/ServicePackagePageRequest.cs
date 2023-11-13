using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class ServicePackagePageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public EnumServicePackageStatus? Status { get; set; }
    }
}
