using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class ActivityInputDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan OccuringTime { get; set; }
        public int OrderIndex { get; set; }
    }
}