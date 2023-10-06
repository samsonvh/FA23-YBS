using YBS.Data.Enums;

namespace YBS.Service.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? Linkedln { get; set; }
        public DateTime ConstractStartDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumCompanyStatus Status { get; set; }
    }
}