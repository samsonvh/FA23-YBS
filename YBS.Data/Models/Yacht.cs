using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Yacht : BaseModel
    {
        public int Id { get; set; }
        public int YachtTypeId { get; set; }
        public YachtType YachtType { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }    
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public float LOA { get; set; }
        public float BEAM { get; set; }
        public float DRAFT { get; set; }
        public string FuelCapacity { get; set; }
        public int MaximumGuestLimit { get; set; }
        public int Cabin { get; set; }
        public EnumYachtStatus Status { get; set; }

    }
}
