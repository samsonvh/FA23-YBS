using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public string Name { get; set; }
        public float TotalPrice { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime PaymentDate { get; set; }
        public EnumPaymentType Type { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public EnumPaymentStatus Status { get; set; }
    }
}