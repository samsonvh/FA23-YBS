using YBS.Data.Enums;

namespace YBS.Service.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string? AvatarURL { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpiredDate { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Status { get; set; }
    }
}