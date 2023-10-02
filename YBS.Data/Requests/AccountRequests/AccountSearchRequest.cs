namespace YBS.Data.Requests.AccountRequests
{
    public class AccountSearchRequest : BaseSearchRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}