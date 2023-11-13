using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class GuestInputDto
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public EnumGender Gender { get; set; }
        public bool IsLeader { get; set; }
        public EnumGuestStatus Status { get; set; }
    }
}