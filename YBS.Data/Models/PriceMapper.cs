using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class PriceMapper
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route Route { get; set; }
        public int YachtTypeId { get; set; }
        [ForeignKey("YachtTypeId")]
        public YachtType YachtType { get; set; }
        public float Price { get; set; }
        public string MoneyUnit { get; set; }
        public double? Point { get; set; }
    }
}