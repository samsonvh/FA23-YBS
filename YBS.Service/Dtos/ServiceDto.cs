using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }    
        public string? Description { get; set; }    
        public string Status { get; set; }
    }
}
