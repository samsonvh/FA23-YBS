using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class MembershipPackagePageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public EnumMembershipPackageStatus? Status { get; set; }
    }
}