using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Services.DataHandler.Dtos
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Beginning { get; set; }
        public string Destination { get; set; }
        public TimeSpan PickupTime { get; set; }
        public TimeSpan StartingTime { get; set; }
        public TimeSpan EndingTime { get; set; }
        public int DurationTime { get; set; }
        public string DurationUnit { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
