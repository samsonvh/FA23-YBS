using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class WalletListingDto
    {
        public int Id { get; set; }
        public float Balance { get; set; }
        public string Status { get; set; }
    }
}
