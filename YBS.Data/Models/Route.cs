
﻿using YBS.Data.Enums;
namespace YBS.Data.Models
{
    public class Route : BaseModel
    {
        public int Id { get; set; } 
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }    
        public string Beginning { get; set; }
        public string Destination { get; set; }
        public TimeSpan PickupTime { get; set; }
        public TimeSpan StartingTime { get; set; }
        public TimeSpan EndingTime { get; set; }
        public int DurationTime { get; set; }
        public string DurationUnit { get; set; }
        public float Price { get; set; }
        public string Unit { get; set; }
        public string Type { get; set; }
        public EnumRouteStatus Status { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<RouteYachtType> RouteYachtTypes { get; set; } = new List<RouteYachtType>();

    }
}
