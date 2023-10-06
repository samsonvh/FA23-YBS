using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }
        public Member Member { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumAccountStatus Status { get; set; }
    }
}
