using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route Route { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan OccuringTime { get; set; }
        public int OrderIndex { get; set; }
        public EnumActivityStatus Status { get; set; }
        public ICollection<ActivityPlace>? ActivityPlaces { get; set; }
    }
}