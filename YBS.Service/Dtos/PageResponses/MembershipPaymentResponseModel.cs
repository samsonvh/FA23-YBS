using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;

namespace CodeMegaVNPay.Models;

public class MembershipPaymentResponseModel
{
    public bool Success { get; set; }
    public int MembershipPackageId { get; set; }
    // All Transaction Field
    public string TransactionName { get; set; }//vnpay return 
    public string TransactionType { get; set; }
    public string PaymentMethod { get; set; }
    public float Amount { get; set; } //vnpay return 
    public string MoneyUnit { get; set; }
    public DateTime PaymentDate { get; set; }
    //All Account Field
    public string Username { get; set; }
    public string Email { get; set; } //vnpay return 
    public string Password { get; set; }
    // All Member Field
    public string FullName { get; set; } //vnpay return 
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }// VNPay return
    public string Nationality { get; set; }
    public EnumGender Gender { get; set; }
    public string Address { get; set; }//vnpay return 
    public string IdentityNumber { get; set; }

    //VNPay Return Field 
    public string TmnCode { get; set; }
    public string TxnRef { get; set; }
    public string ResponseCode { get; set; }
    public string BankCode { get; set; }
    public string cardType { get; set; }
    public string TransactionNo { get; set; }
    public string TransactionStatus { get; set; }
    public string SecureHash { get; set; }
}