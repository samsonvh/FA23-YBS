using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class RouteYachtType
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public int YachtTypeId { get; set; }
        public YachtType YachtType { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
    }
}
