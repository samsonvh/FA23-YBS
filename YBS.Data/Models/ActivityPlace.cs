using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class ActivityPlace
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }
        public int? FromDockId { get; set; }
        [ForeignKey("FromDockId")]
        public virtual Dock? FromDock { get; set; }
        public int? ToDockId { get; set; }
        [ForeignKey("ToDockId")]
        public virtual Dock? ToDock { get; set; }
        public float? StartLocationLatitude { get; set; }
        public float? StartLocationLongtiude { get; set; }
        public float? EndLocationLatitude { get; set; }
        public float? EndLocationLongtiude { get; set; }
    }
}