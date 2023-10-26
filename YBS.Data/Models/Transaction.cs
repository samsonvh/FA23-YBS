using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }
        public string Code { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime CreationDate { get; set; }
        public EnumTransactionStatus Status { get; set; }
    }
}