using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.ListingDtos
{
    public class MembershipPackageListingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string MoneyUnit { get; set; }
        public float Point { get; set; }
        public string Status { get; set; }
    }
}