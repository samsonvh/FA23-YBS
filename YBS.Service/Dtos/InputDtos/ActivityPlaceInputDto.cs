using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class ActivityPlaceInputDto
    {
        public int? FromDockId { get; set; }
        public int? ToDockId { get; set; }
        public float? StartLocationLatitude { get; set; }
        public float? StartLocationLongtiude { get; set; }
        public float? EndLocationLatitude { get; set; }
        public float? EndLocationLongtiude { get; set; }
    }
}