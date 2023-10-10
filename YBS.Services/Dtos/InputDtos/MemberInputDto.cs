using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Services.Dtos.Requests
{
    public class MemberInputDto
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string? AvatarUrl { get; set; }
        public EnumGender Gender { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpiredDate { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}