using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos
{
    public class BookingPaymentDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Name { get; set; }
        public float TotalPrice { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
    }
}