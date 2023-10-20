using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public EnumGender Gender { get; set; }
        public bool IsLeader { get; set; }
        public EnumGuestStatus Status { get; set; }
    }
}