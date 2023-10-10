using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class DockYachtType
    {
        public int Id { get; set; }
        public int DockId { get; set; }
        public Dock Dock { get; set; }
        public int YachtTypeId { get; set; }
        public YachtType YachtType { get; set; }
    }
}