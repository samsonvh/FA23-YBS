using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class TransactionInputDto
    {
        //transaction field
        public int PaymentId { get; set; }
        public string Name { get; set; }
        public EnumTransactionType Type { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        //VNPay return field
        public string VNPayTmnCode { get; set; }
        public string VNPayTxnRef { get; set; }
        public string VNPayResponseCode { get; set; }
        public string VNPAYBankCode { get; set; }
        public string VNPAYcardType { get; set; }
        public string VNPAYTransactionNo { get; set; }
        public string VNPayTransactionStatus { get; set; }
    }
}