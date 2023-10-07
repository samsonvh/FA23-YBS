using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class UpdateRequest
    {
        public int Id { get; set; }
        public int? CompanyID { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumUpdateRequest Status { get; set; }
    }
}