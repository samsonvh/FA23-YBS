using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Beginning { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedPickupTime { get; set; }
        public DateTime ExpectedStartingTime { get; set; }
        public DateTime ExpectedEndingTime { get; set; }
        public int ExpectedDurationTime { get; set; }
        public string DurationUnit { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
