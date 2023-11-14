using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class DealListingDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        public string Departs { get; set; }
        public float Rating { get; set; }
        public string Price { get; set; }
        public string Unit { get; set; }
    }
}
