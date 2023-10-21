using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.PageRequests
{
    public class BookingPageRequest : DefaultPageRequest
    {
        public string? Trip { get; set; }
        public string? Yacht { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateBook { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public EnumBookingStatus? Status { get; set; }
    }
}