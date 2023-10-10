
using YBS.Data.Enums;

namespace YBS.Service.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? FacebookURL { get; set; }
        public string? InstagramURL { get; set; }
        public string? LinkedInURL { get; set; }
        public DateTime ContractStartDate { get; set; }
        public string Status { get; set; }
    }
}

