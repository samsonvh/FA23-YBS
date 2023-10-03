using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }    
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public EnumServiceStatus Status { get; set; }
    }
}
