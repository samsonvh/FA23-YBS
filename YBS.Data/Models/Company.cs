using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Company : BaseModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramURL { get; set; }
        public string? LinkedInURL { get; set; }
        public DateTime ContractStartDate { get; set; }
        public ICollection<Route> Routes { get; set; } = new List<Route>();
        public EnumCompanyStatus Status { get; set; }
    }
}
