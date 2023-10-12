using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos
{
    public class YachtDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Manufacture { get; set; }
        public int Year { get; set; }
        public float LOA { get; set; }
        public float BEAM { get; set; }
        public float DRAFT { get; set; }
        public string FuelCapacity { get; set; }
        public int MaximumGuestLimit { get; set; }
        public int Cabin { get; set; }
        public string Status { get; set; }
    }
}
