using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public EnumServiceType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MoneyUnit { get; set; }
        public EnumServiceStatus Status { get; set; }
        public ICollection<ServicePackageItem> ServicePackageItems { get; set; }
    }
}