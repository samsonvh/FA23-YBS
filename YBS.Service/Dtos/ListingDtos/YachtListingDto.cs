using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class YachtListingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> ImageURL { get; set; }
        public int MaximumGuestLimit { get; set; }
        public int TotalCrew { get; set; }
        public int Cabin { get; set; }
        public string Status { get; set; }
    }
}
