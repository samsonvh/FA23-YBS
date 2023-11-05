using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.ListingDtos
{
    public class MembershipRegistrationListingDto
    {
        public int Id { get; set; } 
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Status { get; set; }
    }
}
