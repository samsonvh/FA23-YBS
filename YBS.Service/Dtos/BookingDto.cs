using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;


namespace YBS.Service.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        //member field
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        //trip field
        public int TripId { get; set; }
        //route field
        public string RouteName { get; set; }
        //booking field
        public int? YachtId { get; set; }
        public string? YachtName { get; set; }
        public string? ServicePackageName { get; set; }
        public DateTime ActualStartingTime { get; set; }
        public DateTime ActualEndingTime { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Note { get; set; }
        public int NumberOfGuest { get; set; }
        public float TotalPrice { get; set; }
        public string MoneyUnit { get; set; }
        public string Status { get; set; }
    }
}


