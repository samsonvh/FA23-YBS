
namespace YBS.Services.DataHandler.Requests.AccountRequests
{
    public class AccountSearchRequest : PageRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}