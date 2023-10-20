using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route Route { get; set; }
        public string Name { get; set; }
        public DateTime ExpectedPickupTime { get; set; }
        public DateTime ExpectedStartingTime { get; set; }
        public DateTime ExpectedEndingTime { get; set; }
        public int ExpectedDurationTime { get; set; }
        public string DurationUnit { get; set; }
        public EnumTripStatus Status { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}