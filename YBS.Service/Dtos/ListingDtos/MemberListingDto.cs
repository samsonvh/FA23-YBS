using YBS.Data.Enums;

namespace YBS.Service.Dtos.ListingDtos
{
    public class MemberListingDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Status { get; set; }
    }
}