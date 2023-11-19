using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class YachtMooringInputDto
    {
        public int YachtId { get; set; }
        public int DockId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime LeaveTime { get; set; }
    }
}