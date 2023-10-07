
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
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public DateTime ContractStartDate { get; set; }
        public EnumCompanyStatus Status { get; set; }


    }
}

