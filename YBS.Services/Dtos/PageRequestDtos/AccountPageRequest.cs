namespace YBS.Services.Dtos.Requests
{
    public class AccountPageRequest : DefaultPageRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}