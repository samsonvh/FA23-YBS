using YBS.Service.Dtos.PageRequests;

namespace YBS.Services.Dtos.Requests
{
    public class AccountPageRequest : DefaultPageRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}