using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class PointPaymentInputDto
    {
        public int RouteId { get; set; }
        public int WalletId { get; set; }
        public List<int>? ListServicePackageId { get; set; }
        public int YachtTypeId { get; set; }
        public string? Note { get; set; }
        public IFormFile? GuestList { get; set; }
        public DateTime OccurDate { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public EnumTransactionType Type { get; set; }
    }
}