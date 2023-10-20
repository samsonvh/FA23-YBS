using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos.InputDtos
{
    public class MemberInputDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int MembershipPackageId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public EnumGender? Gender { get; set; }
        public string? AvatarURL { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public EnumMemberStatus? Status { get; set; }
    }
}