using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Company
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public Account Account { get; set; }    
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string Logo { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? Linkedln { get; set; }
        public DateTime ConstractStartDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumStatus Status { get; set; }
    }
}
