using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class GuestPageRequest : DefaultPageRequest
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public EnumGender? Gender { get; set; }
        public EnumGuestStatus? Status { get; set; }
    }
}