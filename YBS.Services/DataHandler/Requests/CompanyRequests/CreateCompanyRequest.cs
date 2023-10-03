using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Data.Requests.CompanyRequests
{
    public class CreateCompanyRequest
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramURL { get; set; }
        public string? LinkedInURL { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
