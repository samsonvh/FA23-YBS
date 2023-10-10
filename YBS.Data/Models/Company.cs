using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Company
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string LinkedInURL { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumCompanyStatus Status { get; set; }
        public ICollection<UpdateRequest> UpdateRequests { get; set; } = new List<UpdateRequest>();
    }
}