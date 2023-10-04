using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingID { get; set; }
        public float TotalPrice { get; set; }
        public string Unit { get; set; }
        public DateTime PaymentDate { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public EnumPaymentStatus Status { get; set; }
        
    }
}