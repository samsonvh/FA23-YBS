using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class YachtMooringListingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string>? ImageURL { get; set; }
        public int MaximumGuestLimit { get; set; }
        public int TotalCrew { get; set; }
        public int Cabin { get; set; }
        public string Status { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}