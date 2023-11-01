using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class MembershipRegistration
    {
        public int Id { get; set; }
        public Transaction Transaction { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int MembershipPackageId { get; set; }
        public MembershipPackage MembershipPackage { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime DateRegistered { get; set; }
        public EnumMembershipRegistrationStatus Status { get; set; }
    }
}