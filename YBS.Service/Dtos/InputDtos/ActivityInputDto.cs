using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Service.Dtos.InputDtos
{
    public class ActivityInputDto
    {
        public int RouteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OccuringTime { get; set; }
        public int OrderIndex { get; set; }
        public List<ActivityPlaceInputDto> activityPlaceInputDtos { get; set; }
    }
}