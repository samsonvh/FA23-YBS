namespace YBS.Service.Dtos.PageRequests
{
    public class AccountPageRequest : DefaultPageRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}