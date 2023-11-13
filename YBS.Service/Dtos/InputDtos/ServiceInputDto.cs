using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class ServiceInputDto
    {
        public int CompanyId { get; set; }
        public EnumServiceType Type { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string MoneyUnit { get; set; }
    }
}
