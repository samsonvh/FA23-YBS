using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class YachtType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumYachtTypeStatus Status { get; set; }
        public ICollection<Yacht> Yachts { get; set; }
        public ICollection<DockYachtType> DockYachtTypes { get; set; }
    }
}