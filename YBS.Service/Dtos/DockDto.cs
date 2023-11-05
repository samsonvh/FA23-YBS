using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos
{
    public class DockDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longtitude { get; set; }
        public string Description { get; set; }
        public List<String> Image { get; set; }
        public EnumDockStatus Status { get; set; }
    }
}