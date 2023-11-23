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
        public int? BookingPaymentId { get; set; }
        [ForeignKey("BookingPaymentId")]
        public BookingPayment? BookingPayment { get; set; }
        public int? WalletId { get; set; }
        [ForeignKey("WalletId")]
        public Wallet? Wallet { get; set; }
        public int? MembershipRegistrationId { get; set; }
        [ForeignKey("MembershipRegistrationId")]
        public MembershipRegistration? MembershipRegistration { get; set; }
        public string Name { get; set; }
        public EnumTransactionType Type { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        public DateTime CreationDate { get; set; }
        //vnpayField
        public string VNPayTmnCode { get; set; }
        public string VNPayTxnRef { get; set; }
        public string VNPayResponseCode { get; set; }
        public string VNPAYBankCode { get; set; }
        public string VNPAYcardType { get; set; }
        public string VNPAYTransactionNo { get; set; }
        public string VNPayTransactionStatus { get; set; }
        public EnumTransactionStatus Status { get; set; }
    }
}