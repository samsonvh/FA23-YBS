using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class RouteInputDto
    {
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Beginning { get; set; }
        public string? Destination { get; set; }
        public TimeOnly ExpectedStartingTime { get; set; }
        public TimeOnly ExpectedEndingTime { get; set; }
        public string? Type { get; set; }
        public EnumRouteStatus Status { get; set; }
    }
}