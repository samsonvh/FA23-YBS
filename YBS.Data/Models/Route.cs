using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Route
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Beginning { get; set; }
        public string Destination { get; set; }
        public TimeSpan ExpectedStartingTime { get; set; }
        public TimeSpan ExpectedEndingTime { get; set; }
        public string Type { get; set; }
        public EnumRouteStatus Status { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<PriceMapper> PriceMappers { get; set; }
    }
}
