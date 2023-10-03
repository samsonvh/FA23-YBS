using Microsoft.AspNetCore.Identity;
using YBS.Data.Enums;
using YBS.Util.DateTracking;

namespace YBS.Data.Models
{
    public class Account : IdentityUser<int>, IDateTracking
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        public Company Company { get; set; }
        public Member Member { get; set; }
        public Role Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public EnumAccountStatus Status { get; set; }
        public DateTime CreationDate { get ; set; }
        public DateTime? LastModifiedDate { get ; set; }
        public bool IsDeleted { get; set; }
    }
}
