using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Account : BaseModel
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        public Company Company { get; set; }
        public Member Member { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public EnumAccountStatus Status { get; set; }
    }
}
