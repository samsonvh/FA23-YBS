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
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public EnumStatus Status { get; set; }
    }
}
