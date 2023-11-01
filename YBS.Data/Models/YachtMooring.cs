using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class YachtMooring
    {
        public int Id { get; set; }
        public int DockId { get; set; }
        [ForeignKey("DockId")]
        public Dock Dock { get; set; }
        public int YachtId { get; set; }
        [ForeignKey("YachtId")]
        public Yacht Yacht { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}