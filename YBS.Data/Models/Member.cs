using System.ComponentModel.DataAnnotations.Schema;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Member
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public int? MembershipPackageId { get; set; }
        [ForeignKey("MembershipPackageId")]
        public MembershipPackage? MembershipPackage { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public EnumGender Gender { get; set; }
        public string AvatarURL { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpiredDate { get; set; }
        public DateTime MembershipSinceDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}