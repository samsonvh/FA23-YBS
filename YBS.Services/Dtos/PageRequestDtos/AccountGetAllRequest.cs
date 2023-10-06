namespace YBS.Services.Dtos.Requests
{
    public class AccountGetAllRequest : PageRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}