using YBS.Data.Enums;
using YBS.Services.DataHandler.Requests.AccountRequests;

namespace YBS.Services.DataHandler.Requests.MemberRequests
{
    public class MemberCreateRequest
    {
        public AccountCreateRequest Account { get; set; }
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfbirth { get; set; }
        public string Nationality { get; set; }
        public string? ImageUrl { get; set; }
        public EnumGender Gender { get; set; }
        public string? Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpiredDate { get; set; }
        public DateTime MemberSinceDate { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}