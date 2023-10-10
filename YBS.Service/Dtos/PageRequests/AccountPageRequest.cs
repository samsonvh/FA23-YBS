using YBS.Data.Enums;
using YBS.Service.Dtos.PageRequests;

namespace YBS.Services.Dtos.PageRequests
{
    public class AccountPageRequest : DefaultPageRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public EnumAccountStatus? Status { get; set; }
    }
}