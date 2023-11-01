using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class BookingServicePackage
    {
        public int Id { get; set; }
        public int ServicePackageId { get; set; }
        [ForeignKey("ServicePackageId")]
        public ServicePackage ServicePackage { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
    }
}