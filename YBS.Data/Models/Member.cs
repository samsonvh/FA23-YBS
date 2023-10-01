using YBS.Data.Extensions.Enums;

namespace YBS.Data.Models
{
    public class Member
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfbirth { get; set; }
        public string Nationality { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpiredDate { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public MemberStatus Status { get; set; }
    }
}
