using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public Trip Trip { get; set; }
        public int? YachtId { get; set; }
        [ForeignKey("YachtId")]
        public Yacht? Yacht { get; set; }
        public int? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }
        public int? ServicePackageId { get; set; }
        [ForeignKey("ServicePackageId")]
        public ServicePackage? ServicePackage { get; set; }
        public int? MembershipPackageId { get; set; }
        [ForeignKey("MembershipPackageId")]
        public MembershipPackage? MembershipPackage { get; set; }
        public int? AgencyId { get; set; }
        [ForeignKey("AgencyId")]
        public Agency? Agency { get; set; }
        public int YachtTypeId { get; set; }
        [ForeignKey("YachtTypeId")]
        public YachtType YachtType { get; set; }
        public string? Note { get; set; }
        public float TotalPrice { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumBookingStatus Status { get; set; }
        public ICollection<Guest> Guests { get; set; }
    }
}