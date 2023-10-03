using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Guest
    {
        public int ID { get; set; }
        public int BookingID { get; set; }
        public Booking Booking { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public EnumGender Gender { get; set; }
        public bool IsLeader { get; set; }
        public  EnumGuestStatus Status { get; set; }
    }
}