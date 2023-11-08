using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class PaymentInformationInputDto
    {
        public int BookingPaymentId { get; set; }
        public EnumTransactionType TransactionType { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public string MoneyUnit { get; set; }
    }
}