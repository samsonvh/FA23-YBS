using YBS.Data.Enums;

namespace CodeMegaVNPay.Models;

public class BookingPaymentResponseModel
{
    public bool Success { get; set; }
    // All Transaction Field
    public int BookingPaymentId { get; set; }
    public string Name { get; set; }
    public string TransactionType { get; set; }
    public string PaymentMethod { get; set; }
    public float Amount { get; set; }
    public string MoneyUnit { get; set; }
    public DateTime PaymentDate { get; set; }
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