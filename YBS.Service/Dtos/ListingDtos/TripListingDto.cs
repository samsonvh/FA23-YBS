using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos.ListingDtos
{
    public class TripListingDto
    {
        public int Id { get; set; }
        public DateTime ActualStartingTime { get; set; }
        public DateTime ActualEndingTime { get; set; }
        public string Status { get; set; }
    }
}
