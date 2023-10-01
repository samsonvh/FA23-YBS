using YBS.Data.Extensions.Enums;

namespace YBS.Data.Models
{
    public class Account 
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public DateTime CreationDate { get; set; }
        public AccountStatus Status { get; set; }
    }
}
