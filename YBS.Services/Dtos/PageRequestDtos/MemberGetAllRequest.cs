namespace YBS.Services.Dtos.Requests
{
    public class MemberGetAllRequest : PageRequest
    { 
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}