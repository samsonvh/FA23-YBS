using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
/*        public int? StartDockId { get; set; }
        public Dock StartDock { get; set; }
        public int? EndDockId { get; set; }
        public Dock EndDock { get; set; }*/
        public string StartCoordinate { get; set; }
        public string EndCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan OccuringTime { get; set; }
        public int OrderIndex { get; set; }
        public ActivityStatusEnum Status { get; set; }
    }
}
