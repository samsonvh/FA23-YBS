using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class BookingListingDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Guest { get; set; }
        public string Trip { get; set; }
        public string? Yacht { get; set; }
        public DateTime DateBook { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float TotalPrice { get; set; }
        public string MoneyUnit { get; set; }
        public string Status { get; set; }
    }
}