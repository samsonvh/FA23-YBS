using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Transaction : BaseModel
    {
        public int ID { get; set; }
        public int WalletID { get; set; }
        public Wallet Wallet { get; set; }
        public int PaymentID { get; set; }
        public Payment Payment { get; set; }
        public string Code { get; set; }
        public float Amount { get; set; }
        public string Unit { get; set; }
        public EnumTransactionType Type { get; set; }
        public EnumTransactionStatus Status { get; set; }

    }
}