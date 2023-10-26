using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class PaymentInformationInputDto
    {
        public int PaymentId { get; set; }
        public string Name { get; set; }
        public EnumPaymentType PaymentType { get; set; }
    }
}