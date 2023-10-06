using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Services.Dtos.ListingDTOs
{
    public class CompanyListingDto
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }    
        public string HotLine { get; set; }
        public EnumCompanyStatus Status { get; set; }
    }
}
