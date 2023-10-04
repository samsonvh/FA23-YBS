using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Booking : BaseModel
    {
        public int Id { get; set; }
        public int RouteID { get; set; }
        public Route Route { get; set; }
        public int YachtID { get; set; }
        public Yacht Yacht { get; set; }
        public int MemberID { get; set; }
        public Member Member { get; set; }
        public int ServicePackageID { get; set; }
        public int MembershipPackageID { get; set; }
        public MembershipPackage MembershipPackage { get; set; }
        public int AgencyID { get; set; }
        public Agency Agency { get; set; }
        public DateTime ExpectedStartingTime { get; set; }
        public DateTime ActualStartingTime { get; set; }
        public string Note { get; set; }
        public float TotalPrice { get; set; }
        public string Unit { get; set; }
        public EnumBookingStatus Status { get; set; }
        public ICollection<Guest> Guests { get; set; } = new List<Guest>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}