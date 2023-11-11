using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class MemberUpdateInputDto
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public EnumGender Gender { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public IFormFile? AvatarImageFile { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}