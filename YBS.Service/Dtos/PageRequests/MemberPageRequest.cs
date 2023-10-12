using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class MemberPageRequest : DefaultPageRequest
    { 
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
    }
}