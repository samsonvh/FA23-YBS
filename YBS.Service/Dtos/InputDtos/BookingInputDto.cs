using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class BookingInputDto
    {
        public int RouteId { get; set; }
        public int? YachtId { get; set; }
        public int? MemberId { get; set; }
        public int? ServicePackageId { get; set; }
        public int? MembershipPackageId { get; set; }
        public int? AgencyId { get; set; }
        public int YachtTypeId { get; set; }
        public string? Note { get; set; }
        public IFormFile? GuestList { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime OccurDate { get; set; }
    }
}