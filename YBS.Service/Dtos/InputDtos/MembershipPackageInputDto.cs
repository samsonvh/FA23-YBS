using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class MembershipPackageInputDto
    {
        public string? Name { get; set; }
        public float? Price { get; set; }
        public string? MoneyUnit { get; set; }
        public string? Description { get; set; }
        public float? Point { get; set; }
        public int? EffectiveDuration { get; set; }
        public string? TimeUnit { get; set; }
        public EnumMembershipPackageStatus Status { get; set; }
    }
}