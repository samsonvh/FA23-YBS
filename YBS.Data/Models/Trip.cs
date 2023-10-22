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
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public DateTime ActualPickupTime { get; set; }
        public DateTime ActualStartingTime { get; set; }
        public DateTime ActualEndingTime { get; set; }
        public EnumTripStatus Status { get; set; }
    }
}