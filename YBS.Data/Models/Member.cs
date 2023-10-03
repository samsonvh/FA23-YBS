using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Member : BaseModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfbirth { get; set; }
        public string Nationality { get; set; }
        public string ImageUrl { get; set; }
        public EnumGender Gender { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpiredDate { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}
