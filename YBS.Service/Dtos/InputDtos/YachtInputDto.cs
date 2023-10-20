using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class YachtInputDto
    {
        public int? CompanyId { get; set; }
        public int? YachtTypeId { get; set; }
        public string? Name { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public string? Description { get; set; }
        public string? Manufacture { get; set; }
        public int? GrossTonnage { get; set; }
        public string? GrossTonnageUnit { get; set; }
        public int? Range { get; set; }
        public string? RangeUnit { get; set; }
        public int? TotalCrew { get; set; }
        public int? CrusingSpeed { get; set; }
        public int? MaxSpeed { get; set; }
        public string? SpeedUnit { get; set; }
        public int? Year { get; set; }
        public string? LOA { get; set; }
        public string? BEAM { get; set; }
        public string? DRAFT { get; set; }
        public int? FuelCapacity { get; set; }
        public string? FuelCapacityUnit { get; set; }
        public int? MaximumGuestLimit { get; set; }
        public int? Cabin { get; set; }
        public EnumYachtStatus? Status { get; set; }
    }
}