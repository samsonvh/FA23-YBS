using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class ServicePackageInputDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public string MoneyUnit { get; set; }
        public List<int> ServiceId { get; set; }
    }
}
