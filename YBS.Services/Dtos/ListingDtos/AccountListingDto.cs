using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos
{
    public class AccountListingDto
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
