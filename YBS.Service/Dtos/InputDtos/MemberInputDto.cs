using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos.InputDtos
{
    public class MemberInputDto
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int MembershipPackageId { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; }
        public EnumGender? Gender { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string IdentityNumber { get; set; }
        public string Status { get; set; }
    }
}