using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos
{
    public class GuestDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public bool IsLeader { get; set; }
        public string Status { get; set; }
        public bool UpdateStatus { get; set; }
    }
}