using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class DockYachtType
    {
        public int Id { get; set; }
        public int DockId { get; set; }
        [ForeignKey("DockId")]
        public Dock Dock { get; set; }
        public int YachtTypeId { get; set; }
        [ForeignKey("YachtTypeId")]
        public YachtType YachtType { get; set; }
    }
}