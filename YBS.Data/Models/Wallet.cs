using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Wallet 
    {
        public int Id { get; set; }
        public int MemberID { get; set; }
        public Member Member { get; set; }
        public float Balance { get; set; }
        public string Unit { get; set; }
        public EnumWalletStatus Status { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
    }
}