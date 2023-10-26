using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class TransactionInputDto
    {
        public int PaymentId { get; set; }
        public string Code { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        public string TransactionStatus { get; set; }
        public string ResponseCode { get; set; }
        public string SecureHash { get; set; }
    }
}