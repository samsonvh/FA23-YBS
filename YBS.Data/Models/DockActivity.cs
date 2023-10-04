using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class DockActivity
    {
        public int Id { get; set; }
        public int? ActivityId { get; set; }
        public Activity Activity { get; set; }  
        public int DockId { get; set; }
        public Dock Dock { get; set; }  
        public EnumTypeDockActivity? Type { get; set; }
        public EnumDockTypeStatus? Status { get; set; }
    }
}
