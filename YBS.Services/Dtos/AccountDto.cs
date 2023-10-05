using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public EnumAccountStatus Status { get; set; }
    }
}
