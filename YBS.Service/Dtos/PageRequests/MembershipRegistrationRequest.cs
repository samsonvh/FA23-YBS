using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class MembershipRegistrationRequest : DefaultPageRequest
    {
        public EnumMembershipRegistrationStatus? Status { get; set; }
    }
}
