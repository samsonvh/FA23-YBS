using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos
{
    public class MembershipRegistrationDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int MembershipPackageId { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Status { get; set; }
    }
}
