using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class PriceMapperListingDto
    {
        public int YachtTypeId { get; set; }
        public int RouteId { get; set; }
        public float Price { get; set; }
        public string MoneyUnit { get; set; }
    }
}