namespace YBS.Services.DataHandler.Requests.AccountRequests
{
    public class AccountCreateRequest
    {
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}